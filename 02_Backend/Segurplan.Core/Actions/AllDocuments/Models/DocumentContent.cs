using System;
using System.Collections.Generic;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.AllDocuments.Models {
    public class DocumentContent {
        public DocumentContent() {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        #region GeneratePlanTenmplateProperties
        public List<RiskAndPreventiveMeasuresDocumentDto> RisksAndPreventiveMeasures { get; set; } = new List<RiskAndPreventiveMeasuresDocumentDto>();
        public bool RisksAndPreventiveMeasuresRender { get; set; } = true;
        public int PlanId { get; set; }
        public string ProjectName { get; set; } = "";
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        public string CreatorName { get; set; } = "";
        public string CreateDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string CheckDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string RevisionDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string ApproverName { get; set; } = "";
        public string RevisorName { get; set; } = "";
        public int ChapterVersion { get; set; }

        public string WorkLocation { get; set; } = "";
        public string Municipality { get; set; } = "";


        public List<PlanChapterDocumentDto> ChaptersHtml { get; set; } = new List<PlanChapterDocumentDto>();
        public int ExecutionTimeMonths { get; set; }
        public double ExecutionBudget { get; set; }
        public double PSSBudget { get; set; }
        public int WorkersNumber { get; set; }
        public bool IsEvaluation { get; set; }


        #endregion
        #region HtmlFields
        public string AnagramaHtml { get; set; }
        public string OrganizationalStructureHtml { get; set; }
        public string WorkDescriptionHtml { get; set; } = "";
        public string ActivityDescriptionHtml { get; set; }
        public string AffectedServicesDescriptionHtml { get; set; }
        public string AssistanceCentersHtml { get; set; }
        public string EmergencyPlanDescriptionHtml { get; set; }
        public string BlueprintsHtml { get; set; }
        public string BlueprintsIndexHtml { get; set; }
        #endregion
        #region EvaluationOfRisk
        // TODO : borrar 
        public PlanChapterDocumentDto Chapter { get; set; } = new PlanChapterDocumentDto();
        public string DocumentTitle { get; set; }
        #endregion
        #region BUDGET
        public List<ApplicationArticleFamily> ArticleFamily { get; set; } = new List<ApplicationArticleFamily>();
        public decimal TotalBudgetPrice { get; set; }
        #endregion
    }
}

