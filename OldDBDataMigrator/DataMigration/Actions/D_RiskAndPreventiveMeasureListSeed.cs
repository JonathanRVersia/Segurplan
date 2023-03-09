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
    public class D_RiskAndPreventiveMeasureListSeed : ISeedInitializer {

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
        private const string MatchContentBetweenCloseAndOpenTag = "(?<=>)(.*?)(?=<)";
        public const string MatchTableContent = "<td(.*?)td>";

        public D_RiskAndPreventiveMeasureListSeed(IMapper mapper, SegurplanContext segurplanContext, SegurplanProduccionContext produccionContext, SeedUtils utils) {
            this.mapper = mapper;
            this.segurplanContext = segurplanContext;
            this.produccionContext = produccionContext;
            this.utils = utils;
        }
        //Original data
        public List<Medidas> medidas;
        public EvaluacionesMedida evaluacionMedida;
        public List<EvaluacionesMedida> evaluacionesMedidas;
        public List<Capitulos> capitulos;
        public List<Subcapitulos> subcapitulos;
        public List<Actividades> actividades;
        public List<Riesgos> riesgos;
        public List<ProbabilidadesRiesgo> probabilidadesRiesgos;
        public List<Gravedades> gravedades;
        public List<NivelesRiesgo> nivelesRiesgos;
        public List<Usuarios> usuarios;

        //Destination data
        public List<Risk> risks = new List<Risk>();
        public List<Chapter> chapters = new List<Chapter>();
        public List<SubChapter> subchapters = new List<SubChapter>();
        public List<Activity> activities = new List<Activity>();
        public List<PreventiveMeasure> preventiveMeasures = new List<PreventiveMeasure>();
        public List<RisksAndPreventiveMeasures> risksAndPreventiveMeasures = new List<RisksAndPreventiveMeasures>();
        public List<RiskAndPreventiveMeasuresMeasures> riskAndPreventiveMeasuresMeasures = new List<RiskAndPreventiveMeasuresMeasures>();
        public List<Seriousness> seriousnesses = new List<Seriousness>();
        public List<Probability> probabilities = new List<Probability>();
        public List<RiskLevel> riskLevels = new List<RiskLevel>();
        public List<User> users = new List<User>();


        public async Task Seed() {

            await GetOriginalData();
            Convert();
            await SetDestinationData();
        }

        public async Task GetOriginalData() {


            medidas = await produccionContext.Medidas.Where(x => x.IdEstado == 2).ToListAsync();
            evaluacionesMedidas = await produccionContext.EvaluacionesMedida.Where(x => x.IdEstado == 2)
                .Include(y => y.IdMedidaNavigation)
                .Include(y => y.IdCapituloNavigation)
                .Include(y => y.IdSubcapituloNavigation)
                .Include(y => y.IdActividadNavigation)
                .Include(y => y.IdEvaluacionNavigation).ToListAsync();

            capitulos = await produccionContext.Capitulos.Where(x => x.IdEstado == 2).ToListAsync();
            subcapitulos = await produccionContext.Subcapitulos.Where(x => x.IdEstado == 2).ToListAsync();
            actividades = await produccionContext.Actividades.Where(x => x.IdEstado == 2).ToListAsync();
            evaluacionMedida = await produccionContext.EvaluacionesMedida.Where(x => x.IdEstado == 2).Include(y => y.IdMedidaNavigation).Include(y => y.IdCapituloNavigation).Include(y => y.IdSubcapituloNavigation).Include(y => y.IdActividadNavigation).FirstOrDefaultAsync();
            riesgos = await produccionContext.Riesgos.Where(x => x.IdEstado == 2).ToListAsync();
            probabilidadesRiesgos = await produccionContext.ProbabilidadesRiesgo.ToListAsync();
            gravedades = await produccionContext.Gravedades.ToListAsync();
            nivelesRiesgos = await produccionContext.NivelesRiesgo.ToListAsync();
            usuarios = await produccionContext.Usuarios.ToListAsync();


            chapters = await segurplanContext.Chapter.ToListAsync();
            subchapters = await segurplanContext.SubChapter.ToListAsync();
            activities = await segurplanContext.Activity.ToListAsync();
            risks = await segurplanContext.Risk.ToListAsync();
            seriousnesses = await segurplanContext.Seriousness.ToListAsync();
            probabilities = await segurplanContext.Probability.ToListAsync();
            riskLevels = await segurplanContext.RiskLevel.ToListAsync();
            users = await segurplanContext.User.ToListAsync();
        }

        public void Convert() {
            preventiveMeasures = mapper.Map<List<PreventiveMeasure>>(medidas);

            //Se limpian medidas en RTF
            foreach (var prevMeasure in preventiveMeasures) {
                if (prevMeasure.Description.Contains("<") || prevMeasure.Description.Contains(">")) {
                    prevMeasure.Description = prevMeasure.Description.Replace("<", "&lt;");
                    prevMeasure.Description = prevMeasure.Description.Replace(">", "&gt;");
                }

                //RTF a HTML
                if (prevMeasure.Description.Contains("\\rtf"))
                    prevMeasure.Description = RtfPipe.Rtf.ToHtml(prevMeasure.Description);

                //Limpieza HTML
                prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchColgroupAndContent, string.Empty);

                if (prevMeasure.Description.Contains("<div") && prevMeasure.Description.Contains("<p")) {
                    prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchAllDiv, string.Empty);
                    prevMeasure.Description = prevMeasure.Description.Replace("</div>", string.Empty);
                }
                
                if (prevMeasure.Description.Contains("<div")) {
                    prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchAllDiv, "<p style=\"font-size:9pt; font-family:Verdana;\">");
                    prevMeasure.Description = prevMeasure.Description.Replace("</div>", "</p>");
                }

                if (prevMeasure.Description.Contains("<p")) {
                    prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchStyle, "style=\"font-size:9pt; font-family:Verdana;\"");
                }

                if (!prevMeasure.Description.Contains("<p"))
                    prevMeasure.Description = $"<p style=\"font-size:9pt; font-family:Verdana;\">{prevMeasure.Description}</p>";

                var match = Regex.Matches(prevMeasure.Description, MatchContentBetweenCloseAndOpenTag);
                foreach (var x in match) {
                    if (x.ToString().Contains("-") || x.ToString().Contains("·")) {
                        //TODO: Cambiar esto por convertir cada línea en una lista prevMeasure.Description?
                        //Esto es para las listas que pueda haber en las medidas preventivas
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchHyphenBetweenSpaces, "<br/>·");
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchDotBetweenCloseTagAndSpace, "<br/>· ");
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchExtraSemicolonAndQuot, string.Empty);
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchStyle, "style=\"font-size:9pt; font-family:Verdana;\"");
                    }
                }

                prevMeasure.Description = prevMeasure.Description.Replace("<p style=\"font-size:9pt; font-family:Verdana;\"><br></p>", "<br/>");

                while (prevMeasure.Description.Contains("<br/><br/>") || prevMeasure.Description.Contains("<br><br>")) {
                    prevMeasure.Description = prevMeasure.Description.Replace("<br/><br/>", string.Empty);
                    prevMeasure.Description = prevMeasure.Description.Replace("<br><br>", string.Empty);
                }
                try {
                    prevMeasure.Description = Regex.Replace(prevMeasure.Description, "<strong(.*?)>", "<b>");
                    prevMeasure.Description = prevMeasure.Description.Replace("</strong>", "</b>");
                    //Limpiar tablas
                    if (prevMeasure.Description.Contains("<tr")) {
                        prevMeasure.Description = prevMeasure.Description.Replace("&quot;", string.Empty);
                        prevMeasure.Description = prevMeasure.Description.Replace("&nbsp;", string.Empty);
                        prevMeasure.Description = prevMeasure.Description.Replace("<sub>", string.Empty);
                        prevMeasure.Description = prevMeasure.Description.Replace("</sub>", string.Empty);
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, "<span\\s*(.*?)>", string.Empty);
                        prevMeasure.Description = prevMeasure.Description.Replace("</span>", string.Empty);
                        prevMeasure.Description = prevMeasure.Description.Replace("> ", ">");
                        prevMeasure.Description = prevMeasure.Description.Replace("< ", "<");
                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, MatchColgroupAndContent, string.Empty);
                        var regTable = Regex.Matches(prevMeasure.Description, MatchTableContent);
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
                                        prevMeasure.Description = Regex.Replace(prevMeasure.Description, $"(?<=>)({regstring})(?=<)", $"<span>{regstring}</span>");
                                        prevMeasure.Description = prevMeasure.Description.Replace("\\(", "(");
                                        prevMeasure.Description = prevMeasure.Description.Replace("\\)", ")");

                                        hechos.Add(reg.ToString());
                                    }
                                }
                            }
                        }
                    }
                    if (prevMeasure.Description.Contains("td style=\"border: 1px solid black;"))
                        prevMeasure.Description = prevMeasure.Description.Replace("td style=\"border: 1px solid black;", "td style=\"border: 1pt solid black; font-size: 9pt");

                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }


            }

            int createdModifiedByUserdId = 1;

            foreach (var evalMed in evaluacionesMedidas) {
                var risksAndPreventiveMeasure = mapper.Map<RisksAndPreventiveMeasures>(evalMed);

                createdModifiedByUserdId = utils.GetUserIdFromNewDB(users, usuarios, evalMed.IdUsuario ?? 1);
                risksAndPreventiveMeasure.CreatedBy = createdModifiedByUserdId;
                risksAndPreventiveMeasure.ModifiedBy = createdModifiedByUserdId;


                if (risksAndPreventiveMeasure.PreventiveMeasures == null)
                    risksAndPreventiveMeasure.PreventiveMeasures = new List<RiskAndPreventiveMeasuresMeasures>();

                if (evalMed.IdMedidaNavigation != null) {
                    var preventiveMeasure = preventiveMeasures.Where(x => x.Code == (evalMed.IdMedidaNavigation.Codigo ?? 0)).FirstOrDefault();

                    risksAndPreventiveMeasure.PreventiveMeasures.Add(new RiskAndPreventiveMeasuresMeasures { PreventiveMeasure = preventiveMeasure });
                }

                var riesgo = riesgos.Where(x => x.Id == evalMed.IdRiesgo).FirstOrDefault();

                Risk risk = new Risk();
                if (riesgo != null) {
                    risk = risks.Where(x => x.Code == riesgo.Codigo).FirstOrDefault();
                }

                if (risk != default) {
                    risksAndPreventiveMeasure.RiskId = risk.Id;
                }



                if (evalMed.IdCapituloNavigation != null) {
                    risksAndPreventiveMeasure.ChapterId = chapters.Where(x => x.Number == evalMed.IdCapituloNavigation.Capitulo).Select(y => y.Id).FirstOrDefault();
                }

                if (evalMed.IdSubcapituloNavigation != null) {
                    risksAndPreventiveMeasure.SubChapterId = subchapters.Where(x => x.IdChapter == risksAndPreventiveMeasure.ChapterId && x.Number == evalMed.IdSubcapituloNavigation.SubCapitulo).Select(y => y.Id).FirstOrDefault();
                }

                if (evalMed.IdActividadNavigation != null) {
                    risksAndPreventiveMeasure.ActivityId = activities.Where(x => x.SubChapterId == risksAndPreventiveMeasure.SubChapterId && x.Number == int.Parse(evalMed.IdActividadNavigation.Actividad)).Select(y => y.Id).FirstOrDefault();
                }


                if (evalMed.IdEvaluacionNavigation != null) {

                    if (evalMed.IdEvaluacionNavigation.IdProbabilidad != null) {
                        var probabilidadRiesgo = probabilidadesRiesgos.Where(x => x.Id == evalMed.IdEvaluacionNavigation.IdProbabilidad).FirstOrDefault();
                        //In our BD is Moderada instead Media
                        if (probabilidadRiesgo.Probabilidad.ToUpper() == "MEDIA")
                            probabilidadRiesgo.Probabilidad = "Moderada";
                        risksAndPreventiveMeasure.ProbabilityId = probabilities.Where(x => x.Value.ToUpper() == probabilidadRiesgo.Probabilidad.ToUpper()).Select(x => x.Id).FirstOrDefault();
                    } else {
                        risksAndPreventiveMeasure.ProbabilityId = probabilities.FirstOrDefault().Id;
                    }

                    if (evalMed.IdEvaluacionNavigation.IdGravedad != null) {
                        var gravedad = gravedades.Where(x => x.Id == evalMed.IdEvaluacionNavigation.IdGravedad).FirstOrDefault();
                        risksAndPreventiveMeasure.SeriousnessId = seriousnesses.Where(x => x.Value.ToUpper() == gravedad.Gravedad.ToUpper()).Select(x => x.Id).FirstOrDefault();
                    } else {
                        risksAndPreventiveMeasure.SeriousnessId = seriousnesses.FirstOrDefault().Id;
                    }

                    if (evalMed.IdEvaluacionNavigation.IdNivelRiesgo != null) {
                        var nivelRiesgo = nivelesRiesgos.Where(x => x.Id == evalMed.IdEvaluacionNavigation.IdNivelRiesgo).FirstOrDefault();
                        risksAndPreventiveMeasure.RiskLevelId = riskLevels.Where(x => x.Level.ToUpper() == nivelRiesgo.Nivel.ToUpper()).Select(x => x.Id).FirstOrDefault();
                    } else {
                        risksAndPreventiveMeasure.RiskLevelId = riskLevels.FirstOrDefault().Id;
                    }

                    risksAndPreventiveMeasure.RiskOrder = evalMed.IdEvaluacionNavigation.OrdenRiesgo ?? 1;
                }

                risksAndPreventiveMeasures.Add(risksAndPreventiveMeasure);
            }

            var checkHowManyRisksAndPreventiveMeasuresHasntActivity = risksAndPreventiveMeasures.Where(x => x.ActivityId == 0).ToList();
            var checkHowManyRisksAndPreventiveMeasuresHasntRisk = risksAndPreventiveMeasures.Where(x => x.RiskId == 0).ToList();
            var checkHowManyRisksAndPreventiveMeasuresHasntProbability = risksAndPreventiveMeasures.Where(x => x.ProbabilityId == 0).ToList();
            var checkHowManyRisksAndPreventiveMeasuresHasntRiskLevel = risksAndPreventiveMeasures.Where(x => x.RiskLevelId == 0).ToList();
            var checkHowManyRisksAndPreventiveMeasuresHasntSeriousness = risksAndPreventiveMeasures.Where(x => x.SeriousnessId == 0).ToList();
            var checkHowManyRisksAndPreventiveMeasuresHasntRiskORder = risksAndPreventiveMeasures.Where(x => x.RiskOrder == 0).ToList();
        }

        public async Task SetDestinationData() {

            risksAndPreventiveMeasures = CleanDuplicatedRiskAndPreventiveMeasures();

            segurplanContext.RemoveRange(segurplanContext.RiskAndPreventiveMeasuresMeasures);

            segurplanContext.RemoveRange(segurplanContext.PreventiveMeasure);
            await segurplanContext.AddRangeAsync(preventiveMeasures);

            segurplanContext.RemoveRange(segurplanContext.RisksAndPreventiveMeasures);
            await segurplanContext.AddRangeAsync(risksAndPreventiveMeasures);

            int changes = await segurplanContext.SaveChangesAsync();

        }

        public List<RisksAndPreventiveMeasures> CleanDuplicatedRiskAndPreventiveMeasures() {
            var cleanedList = new List<RisksAndPreventiveMeasures>();

            var groupedList = risksAndPreventiveMeasures.GroupBy(x => new { x.ActivityId, x.ChapterId, x.SubChapterId, x.RiskId }).ToList();

            foreach (var group in groupedList) {
                var groupRiskPrevList = group.ToList();

                var riskPrevCleaned = groupRiskPrevList.First();
                riskPrevCleaned.PreventiveMeasures = groupRiskPrevList.SelectMany(x => x.PreventiveMeasures).ToList();
                cleanedList.Add(riskPrevCleaned);
            }

            return cleanedList;
        }
    }
}
