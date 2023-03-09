using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OldDBDataMigrator.DataMigration.CleanProducedBy;
using OldDBDataMigrator.DataMigration.CleanTexts;
using OldDBDataMigrator.DataMigration.ConsoleSeeds;
using OldDBDataMigrator.DataMigration.PreventiveMeasuresOrderUpdate;
using OldDBDataMigrator.DataMigration.DuplicateRiskAssignments;
using OldDBDataMigrator.DataMigration.Templates;
using OldDBDataMigrator.Utils;

namespace OldDBDataMigrator {
    public class RunApplication {

        private readonly IEnumerable<ISeedInitializer> seedInitializers;
        private readonly SeedUtils utils;
        private readonly UsersConsoleProgram usersConsoleProgram;
        private readonly UpdateTemplates updateTemplates;
        private readonly UpdateAnagram updateAnagram;
        private readonly TextCleaner textCleaner;
        private readonly ProducedByCleaner producedByCleaner;
        private readonly UpdatePreventiveMeasuresOrder updatePreventiveMeasuresOrder;
        private readonly DuplicateRiskAssignments duplicateRiskAssignments;

        public RunApplication(
            IEnumerable<ISeedInitializer> seedInitializers,
            SeedUtils utils,
            UsersConsoleProgram usersConsoleProgram,
            UpdateTemplates updateTemplates,
            TextCleaner textCleaner,
            ProducedByCleaner producedByCleaner,
            UpdateAnagram updateAnagram,
            UpdatePreventiveMeasuresOrder updatePreventiveMeasuresOrder,
            DuplicateRiskAssignments duplicateRiskAssignments) {
            this.seedInitializers = seedInitializers;
            this.utils = utils;
            this.usersConsoleProgram = usersConsoleProgram;
            this.updateTemplates = updateTemplates;
            this.textCleaner = textCleaner;
            this.producedByCleaner = producedByCleaner;
            this.updateAnagram = updateAnagram;
            this.updatePreventiveMeasuresOrder = updatePreventiveMeasuresOrder;
            this.duplicateRiskAssignments = duplicateRiskAssignments;
        }

        public async Task Run(CancellationToken cancellationToken) {

            bool validKey = false;

            while (!validKey) {

                string userInput = utils.PrintMessageAndReadKey(
                    "Elija una opción: " +
                    "\r\n 1.Ejecutar Seed " +
                    "\r\n 2.Usuarios (necesita conexión con AD, introduzca usuario y contraseña en appsettings.development)" +
                    "\r\n 3.Actualizar Templates (si no funcionase mirar en propiedades de Templates que el word se copia)" +
                    "\r\n 4.Limpiar textos de la base de datos actual" +
                    "\r\n 5.Limpiar Elaborado Por duplicados de Capítulos (eliminar funcionalidad tras limpiar BDs)" +
                    "\r\n 6.Ejecutar migraciones (sin hacer)" +
                    "\r\n 7.Actualizar anagrama" +
                    "\r\n 8.Actualizar orden de medidas"+
                    "\r\n 9.Duplicar medidas de vigente a borrador");

                validKey = Enum.TryParse(userInput, out SeedResponse result);

                switch (result) {
                    case SeedResponse.InitialSeed:
                        await SeedFromOldDb();
                        break;
                    case SeedResponse.Users:
                        await usersConsoleProgram.Initialize();
                        break;
                    case SeedResponse.Templates:
                        await updateTemplates.Initialize();
                        break;
                    case SeedResponse.CleanTexts:
                        await textCleaner.Initialize();
                        break;
                    case SeedResponse.CleanProducedByLists:
                        await producedByCleaner.Initialize();
                        break;
                    case SeedResponse.UpdateAnagram:
                        await updateAnagram.Initialize();
                        break;
                    case SeedResponse.UpdatePreventiveMeasuresOrder:
                        await updatePreventiveMeasuresOrder.Initialize();
                        break;
                    case SeedResponse.DuplicateRiskAssignments:
                        await duplicateRiskAssignments.Initialize();
                        break;
                    case SeedResponse.ExecuteMigrations:
                        break;

                }

                if (!validKey)
                    utils.PrintErrorMessage("Comando no válido");
            }

        }

        private async Task SeedFromOldDb() {
            foreach (var classInitalizer in seedInitializers) {
                await classInitalizer.Seed();
            }
        }

        private enum SeedResponse {
            InitialSeed = 1,
            Users = 2,
            Templates = 3,
            CleanTexts = 4,
            CleanProducedByLists = 5,
            ExecuteMigrations = 6,
            UpdateAnagram = 7,
            UpdatePreventiveMeasuresOrder=8,
            DuplicateRiskAssignments = 9
        }
    }
}
