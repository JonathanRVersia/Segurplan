using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.ProduccionDBModels;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;

namespace OldDBDataMigrator.DataMigration.Actions {
    public class C_ChaptersSubChaptersAndActivitiesSeed : ISeedInitializer {

        private readonly IMapper mapper;
        private readonly SegurplanContext segurplanContext;
        private readonly SegurplanProduccionContext produccionContext;
        private readonly SeedUtils utils;

        private const string MatchColgroupAndContent = "<colgroup>(.*?)<\\/colgroup>";
        private const string MatchHyphenBetweenSpaces = "\\s-\\s";
        public const string MatchDotBetweenCloseTagAndSpace = "(?<=>)·\\s";
        public const string MatchExtraSemicolonAndQuot = "(?<=\");\"";
        public const string MatchStyle = "(style=\")(.*?)(\\\")";
        public const string MatchAllFontSize = "(font-size:)(.*?)(?=;|\")";
        public const string MatchAllDiv = "(<div)(.*?)(>)";
        public const string MatchTableContent = "<td(.*?)td>";
        private const string MatchContentBetweenCloseAndOpenTag = "(?<=>)(.*?)(?=<)";

        public C_ChaptersSubChaptersAndActivitiesSeed(SeedUtils utils, IMapper mapper, SegurplanContext segurplanContext, SegurplanProduccionContext produccionContext) {
            this.mapper = mapper;
            this.segurplanContext = segurplanContext;
            this.produccionContext = produccionContext;
            this.utils = utils;
        }

        public List<Capitulos> capitulos;
        public List<Subcapitulos> subcapitulos;
        public List<Actividades> actividades;
        public List<Usuarios> usuarios;

        public List<Chapter> chapters = new List<Chapter>();
        public List<SubChapter> subChapters = new List<SubChapter>();
        public List<Activity> activities = new List<Activity>();
        public List<ChapterVersion> chapterVersions = new List<ChapterVersion>();
        public List<SubChapterVersion> subChapterVersions = new List<SubChapterVersion>();
        public List<ActivityVersion> activityVersions = new List<ActivityVersion>();
        public List<User> users = new List<User>();

        public List<Chapter> newChapters = new List<Chapter>();

        public async Task Seed() {
            await GetOriginalData();
            Convert();
            await SetDestinationData();
        }

        public async Task GetOriginalData() {
            capitulos = await produccionContext.Capitulos.Where(x => x.IdEstado == 2).ToListAsync();
            subcapitulos = await produccionContext.Subcapitulos.Where(x => x.IdEstado == 2).ToListAsync();
            actividades = await produccionContext.Actividades.Where(x => x.IdEstado == 2).ToListAsync();
            usuarios = await produccionContext.Usuarios.ToListAsync();

            chapters = await segurplanContext.Chapter.ToListAsync();
            subChapters = await segurplanContext.SubChapter.ToListAsync();
            activities = await segurplanContext.Activity.ToListAsync();
            users = await segurplanContext.User.ToListAsync();
        }

        public void Convert() {
            int createdByUserId = 1;
            int modifiedByUserId = 1;
            int reviewedByUserId = 1;
            int approbedByUserId = 1;

            foreach (var capitulo in capitulos) {
                var newChapter = mapper.Map<Chapter>(capitulo);

                if (newChapter.Number == 1) {
                    newChapter.DefaultSelectedChapter = true;
                }

                createdByUserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdElaborador1 ?? 1);
                modifiedByUserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdElaborador1 ?? 1);

                newChapter.CreatedBy = createdByUserId;
                newChapter.ModifiedBy = modifiedByUserId;

                var chapterVersions = CreateChapterVersions(capitulo);

                foreach (var version in chapterVersions) {

                    version.CreatedBy = createdByUserId;
                    version.ModifiedBy = modifiedByUserId;

                    if (capitulo.IdComprobador != null) {
                        reviewedByUserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdComprobador ?? 1);
                        version.IdReviewer = reviewedByUserId;
                    }

                    if (version.ProducedBy == null)
                        version.ProducedBy = new List<UserChapterVersion>();

                    if (capitulo.IdElaborador1 != null) {
                        version.ProducedBy.Add(new UserChapterVersion { UserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdElaborador1 ?? 1) });
                    }

                    if (capitulo.IdElaborador2 != null) {
                        version.ProducedBy.Add(new UserChapterVersion { UserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdElaborador2 ?? 1) });
                    }

                    if (capitulo.IdAprobador != null) {
                        approbedByUserId = utils.GetUserIdFromNewDB(users, usuarios, capitulo.IdAprobador ?? 1);
                        version.IdApprover = approbedByUserId;
                    }

                    if (newChapter.ChapterVersion == null)
                        newChapter.ChapterVersion = new List<ChapterVersion>();

                    newChapter.ChapterVersion.Add(version);
                }

                var subcapitulosHijos = subcapitulos.Where(x => x.IdCapitulo == capitulo.Id).ToList();

                foreach (var subcapitulo in subcapitulosHijos) {

                    var newSubChapter = mapper.Map<SubChapter>(subcapitulo);

                    var subChapterVersion = mapper.Map<SubChapterVersion>(subcapitulo);

                    createdByUserId = utils.GetUserIdFromNewDB(users, usuarios, subcapitulo.IdUsuario ?? 1);
                    modifiedByUserId = utils.GetUserIdFromNewDB(users, usuarios, subcapitulo.IdUsuario ?? 1);

                    newSubChapter.CreatedBy = createdByUserId;
                    newSubChapter.ModifiedBy = modifiedByUserId;

                    subChapterVersion.CreatedBy = createdByUserId;
                    subChapterVersion.ModifiedBy = modifiedByUserId;

                    if (capitulo.IdComprobador != null) {
                        subChapterVersion.IdReviewer = reviewedByUserId;
                    }

                    if (capitulo.IdAprobador != null) {
                        subChapterVersion.IdApprover = approbedByUserId;
                    }
                    var maxVersionNumber = chapterVersions.Max(x => x.VersionNumber);

                    subChapterVersion.IdChapterVersionNavigation = chapterVersions.Where(x => x.VersionNumber == maxVersionNumber).FirstOrDefault();
                    if (subChapterVersion.Description == "              ") {
                        subChapterVersion.Description = null; 
                    }
                    if (newSubChapter.Description == "              ") {
                        newSubChapter.Description = null;
                    }
                   
                    try {
                        #region Limpiar WorkDetails
                        if (subChapterVersion.WorkDetails != null) {
                            subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("<", "&lt;");
                            subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace(">", "&gt;");

                            //Convertir a HTML
                            if (subChapterVersion.WorkDetails.Contains("\\rtf"))
                                subChapterVersion.WorkDetails = RtfPipe.Rtf.ToHtml(subChapterVersion.WorkDetails);

                            //Limpieza HTML
                            subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchColgroupAndContent, string.Empty);
                            subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchAllDiv, "<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">");
                            subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("</div>", "</p>");
                            if (!subChapterVersion.WorkDetails.Contains("<p"))
                                subChapterVersion.WorkDetails = $"<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">{ subChapterVersion.WorkDetails}</p>";
                            if (subChapterVersion.WorkDetails.Contains("-") || subChapterVersion.WorkDetails.Contains("·")) {
                                //TODO: Cambiar esto por convertir cada línea en una lista subChapterVersion.WorkDetails?
                                //Esto es para las listas que pueda haber en las medidas preventivas
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchHyphenBetweenSpaces, "<br/>·");
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchDotBetweenCloseTagAndSpace, "<br/>· ");
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchExtraSemicolonAndQuot, string.Empty);
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchStyle, "style=\"font-size:9pt; font-family:Verdana, sans-serif;\"");
                            }

                            subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("<p style=\"font-size:9pt; font-family:Verdana, sans-serif;\"><br></p>", "<br/>");

                            while (subChapterVersion.WorkDetails.Contains("<br/><br/>") || subChapterVersion.WorkDetails.Contains("<br><br>")) {
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("<br/><br/>", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("<br><br>", string.Empty);
                            }

                            subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, "<strong(.*?)>", "<b>");
                            subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("</strong>", "</b>");

                            //Limpiar tablas
                            if (subChapterVersion.WorkDetails.Contains("<tr")) {
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("&quot;", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("&nbsp;", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("<sub>", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("</sub>", string.Empty);
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, "<span\\s*(.*?)>", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("</span>", string.Empty);
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("> ", ">");
                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("< ", "<");
                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, MatchColgroupAndContent, string.Empty);
                                var regTable = Regex.Matches(subChapterVersion.WorkDetails, MatchTableContent);
                                MatchCollection regex;
                                List<string> hechos = new List<string>();
                                hechos.Add(" ");
                                foreach (var tableData in regTable) {
                                    string tableDataString = tableData.ToString();

                                    regex = Regex.Matches(tableDataString, MatchContentBetweenCloseAndOpenTag);
                                    foreach (var reg in regex) {
                                        string regstring = reg.ToString();
                                        if (!hechos.Contains(regstring)) {
                                            if (regstring != string.Empty) {

                                                regstring = regstring.Replace("(", "\\(");
                                                regstring = regstring.Replace(")", "\\)");
                                                subChapterVersion.WorkDetails = Regex.Replace(subChapterVersion.WorkDetails, $"(?<=>)({regstring})(?=<)", $"<span>{regstring}</span>");
                                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("\\(", "(");
                                                subChapterVersion.WorkDetails = subChapterVersion.WorkDetails.Replace("\\)", ")");

                                                hechos.Add(reg.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        //No se limpia Description que corrresponde al campo Texto, porque su valor lo estamos metiendo en Descripcion de los trabajos(workdetails)
                        //#region Limpiar Description
                        //if (subChapterVersion.Description != null) {

                        //    //Convertir a HTML
                        //    if (subChapterVersion.Description.Contains("\\rtf"))
                        //        subChapterVersion.Description = RtfPipe.Rtf.ToHtml(subChapterVersion.Description);

                        //    //Limpieza HTML
                        //    subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchColgroupAndContent, string.Empty);
                        //    subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchAllDiv, "<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">");
                        //    subChapterVersion.Description = subChapterVersion.Description.Replace("</div>", "</p>");

                        //    if (subChapterVersion.Description.Contains("<div") && subChapterVersion.Description.Contains("<p")) {
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchAllDiv, string.Empty);
                        //        subChapterVersion.Description = subChapterVersion.Description.Replace("</div>", string.Empty);
                        //    }

                        //    if (subChapterVersion.Description.Contains("<div")) {
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchAllDiv, "<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">");
                        //        subChapterVersion.Description = subChapterVersion.Description.Replace("</div>", "</p>");
                        //    }

                        //    if (subChapterVersion.Description.Contains("<p")) {
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchStyle, "style=\"font-size:9pt; font-family:Verdana, sans-serif;\"");
                        //    }

                        //    if (!subChapterVersion.Description.Contains("<p"))
                        //        subChapterVersion.Description = $"<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">{ subChapterVersion.Description}</p>";

                        //    //Por si tiene listas de algun tipo
                        //    if (subChapterVersion.Description.Contains("-") || subChapterVersion.Description.Contains("·")) {
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchHyphenBetweenSpaces, "<br/>·");
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchDotBetweenCloseTagAndSpace, "<br/>· ");
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchExtraSemicolonAndQuot, string.Empty);
                        //        subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, MatchStyle, "style=\"font-size:9pt; font-family:Verdana, sans-serif;\"");
                        //    }

                        //    subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, "<strong(.*?)>", "<b>");
                        //    subChapterVersion.Description = subChapterVersion.Description.Replace("</strong>", "</b>");
                        //    subChapterVersion.Description = Regex.Replace(subChapterVersion.Description, "<span\\s*(.*?)>", string.Empty);
                        //    subChapterVersion.Description = subChapterVersion.Description.Replace("</span>", string.Empty);

                        //    subChapterVersion.Description = subChapterVersion.Description.Replace("<p style=\"font-size:9pt; font-family:Verdana, sans-serif;\"><br></p>", "<br/>");

                        //    while (subChapterVersion.Description.Contains("<br/><br/>") || subChapterVersion.Description.Contains("<br><br>")) {
                        //        subChapterVersion.Description = subChapterVersion.Description.Replace("<br/><br/>", string.Empty);
                        //        subChapterVersion.Description = subChapterVersion.Description.Replace("<br><br>", string.Empty);
                        //    }

                        //    if(subChapterVersion.Description == "<p style=\"font-size:9pt; font-family:Verdana, sans-serif;\"><br/></p>") {
                        //        subChapterVersion.Description = null;
                        //    }
                        //}
                        //#endregion

                        newSubChapter.SubChapterVersion.Add(subChapterVersion);
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                    var actividadesHijas = actividades.Where(x => x.IdSubcapitulo == subcapitulo.Id).ToList();

                    foreach (var actividad in actividadesHijas) {
                        var newActivity = mapper.Map<Activity>(actividad);
                        var activityVersion = mapper.Map<ActivityVersion>(actividad);

                        createdByUserId = utils.GetUserIdFromNewDB(users, usuarios, actividad.IdUsuario ?? 1);
                        modifiedByUserId = utils.GetUserIdFromNewDB(users, usuarios, actividad.IdUsuario ?? 1);

                        newActivity.CreatedBy = createdByUserId;
                        newActivity.ModifiedBy = modifiedByUserId;

                        activityVersion.CreatedBy = createdByUserId;
                        activityVersion.ModifiedBy = modifiedByUserId;

                        if (capitulo.IdComprobador != null) {
                            activityVersion.IdReviewer = reviewedByUserId;
                        }

                        if (capitulo.IdAprobador != null) {
                            activityVersion.IdApprover = approbedByUserId;
                        }

                        activityVersion.IdSubChapterVersionNavigation = subChapterVersion;

                        newActivity.ActivityVersion.Add(activityVersion);

                        newSubChapter.Activity.Add(newActivity);
                    }
                    newChapter.SubChapter.Add(newSubChapter);
                }
                newChapters.Add(newChapter);
            }
        }


        public async Task SetDestinationData() {

            if (newChapters.Any()) {
                await segurplanContext.AddRangeAsync(newChapters);
                int changes = await segurplanContext.SaveChangesAsync();
            }
        }

        private List<ChapterVersion> CreateChapterVersions(Capitulos capitulo) {
            var chapterVersions = new List<ChapterVersion>();

            if (capitulo.NumRevision > 1) {
                for (int i = 0; i < capitulo.NumRevision; i++) {
                    var chapterVersion = mapper.Map<ChapterVersion>(capitulo);
                    chapterVersion.VersionNumber = i + 1;

                    if (i + 1 != capitulo.NumRevision)
                        chapterVersion.EndDate = DateTime.MinValue;
                    //poner fechas diferentes en todas las Vs menos en la última
                    chapterVersions.Add(chapterVersion);
                }
            } else {
                chapterVersions.Add(mapper.Map<ChapterVersion>(capitulo));
            }


            return chapterVersions;
        }
    }
}
