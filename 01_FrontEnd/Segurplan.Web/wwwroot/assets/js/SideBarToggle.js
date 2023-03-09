var thisPageUrl = window.location.pathname.replace(/\//g, '');

//toggleBehaviour(thisPageUrl);
window.onloadend = toggleBehaviour(thisPageUrl);
function toggleBehaviour(url) {

    let planCaretBtn = document.getElementById("plansCaret");
    let riskCaretBtn = document.getElementById("riskCaret");
    let adminCaretBtn = document.getElementById("adminCaret");

    let adminCaret = document.getElementById("pagesAdmin");
    let riskCaret = document.getElementById("RisksEvaluations");
    let plansCaret = document.getElementById("pagesExamples");

    switch (url) {
        //Plan cases
        case 'AllPlans':
        case 'PlanManagement':
        case 'MyPlans':
            if (adminCaret != null && adminCaret.classList.contains("show")) {
                adminCaretBtn.click();
            }
            if (riskCaret.classList.contains("show")) {
                riskCaretBtn.click();
            }
            if (!plansCaret.classList.contains("show")) {
                planCaretBtn.click();
            }
            break;

        //administration cases
        default:
        case 'ArticlesList':
        case 'ArticleManagement':
        case 'ArticleFamilyList':
        case 'ArticleFamilyManagement':
        case 'TasksList':
        case 'TaskManagement':
        case 'TemplateList':
        case 'TemplateManagement':
        case 'RiskList':
        case 'RiskManagement':
        case 'SeriousnessList':
        case 'SeriousnessManagement':
        case 'PreventiveMeasureList':
        case 'MeasureManagement':
        case 'UserList':
        case 'UserManagement':
        case "ActivityList":
        case "ActivityManagement":
        case "ModelsAdministrationChaptersAndActivitiesChapterDetails":
        case "ModelsAdministrationChaptersAndActivitiesSubChapterDetails":
        case "ModelsAdministrationChaptersAndActivitiesActivityDetails":
        case "RisksAndPreventiveMeasures":
        case "RisksAndPreventiveMeasuresDetails":
        case "ModelsRisksEvaluationAllocationOfRisksAndPreventiveMeasuresDetailsIndex1true":
        case "ModelsRisksEvaluationAllocationOfRisksAndPreventiveMeasuresDetails0true":

            if (!adminCaret.classList.contains("show")) {
                adminCaretBtn.click();
            }
            if (riskCaret.classList.contains("show")) {
                riskCaretBtn.click();
            }
            if (plansCaret.classList.contains("show")) {
                planCaretBtn.click();
            }
            break;
        //Risk evaluation cases
        case "RiskAssessmentsAndMaps":
            if (adminCaret != null && adminCaret.classList.contains("show")) {
                adminCaretBtn.click();
            }
            if (!riskCaret.classList.contains("show")) {
                riskCaretBtn.click();
            }
            if (plansCaret.classList.contains("show")) {
                planCaretBtn.click();
            }

            break;

    }
}