using Segurplan.Core.Actions.AllDocuments.Models;
using System;

namespace Segurplan.Core.Actions.AllDocuments.Helpers {
    public static class CustomBusinessMappings {

        public static void FillRisksPerActivityCharsData(DocumentContent planTemplateData) {
            bool cleanActName;

            foreach (var chapter in planTemplateData.ChaptersHtml) {
                foreach (var subChapter in chapter.SubChaptersHtml) {
                    foreach (var act in subChapter.ActivitiesHtml) {
                        cleanActName = false;
                        foreach (var actRisk in act.MeasuresPerRiskAndActivityHtml) {

                            // Borramos los nombres de las actividades menos en el primer caso para que no se repitan en el doc
                            if (cleanActName)
                                actRisk.ActivityName = string.Empty;

                            if (!string.IsNullOrEmpty(actRisk.RiskLevelLevel)) {
                                actRisk.RiskLevelChar = actRisk.RiskLevelLevel.ToUpper() == "INTOLERABLE"
                                    ||
                                    actRisk.RiskLevelLevel.ToUpper() == "TOLERABLE"
                                    ?
                                    actRisk.RiskLevelLevel.ToUpper().Substring(0, 2)
                                    :
                                    actRisk.RiskLevelLevel.ToUpper().Substring(0, 1);
                            }

                            if (!string.IsNullOrEmpty(actRisk.SeriousnessValue))
                                actRisk.SeriousnessChar = actRisk.SeriousnessValue.GetFirstChars().ToUpper();


                            if (!string.IsNullOrEmpty(actRisk.ProbabilityValue))
                                actRisk.ProbabilityChar = actRisk.ProbabilityValue.GetFirstChars().ToUpper();


                            cleanActName = true;
                        }
                    }
                }
            }
        }
    }
}
