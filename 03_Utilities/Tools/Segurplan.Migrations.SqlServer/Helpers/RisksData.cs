﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Migrations.SqlServer.Helpers {
    public static class RisksData {
        public static Dictionary<int, string> Risks = new Dictionary<int, string> {
                {96,  " Ver riesgos en \" 8.2.7 (Tendido de cable y fibra óptica: en tejado/cubiertas y azoteas)\"." },
                {0 ,  "Accidentes de tráfico" },
                {1 ,  "Agentes Biológicos" },
                {2 , "Agentes Químicos" },
                {3  ,"Agresión de Animales"},
                {157," Agresión física" },
                {61 ," Ahogamiento" },
                {4  ," Ambientes pulverulentos" },
                {188," Aplastamientos y atrapamientos" },
                {5  ," Arco Eléctrico" },
                {65 ," Arrollamiento" },
                {156," Asfixia"},
                {160," Atrapamiento por partes móviles" },
                {190," Atrapamiento por y/o entre objetos" },
                {6  ," Atrapamientos" },
                {213," Atrapamientos entre los tubos y las paredes de la zanja" },
                {159," Atrapamientos por vuelco" },
                {228," Atrapamientos por vuelco de la maquinaria" },
                {7  ," Atropellos" },
                {206," Atropellos, colisiones, vuelcos y falsas maniobras de la máquina" },
                {229," Atropellos, golpes y choques con/contra vehículos" },
                {231," Atropellos, golpes y choques contra objetos móviles e inmóviles " },
                {198," Atropellos, golpes y colisiones con maquinaria o vehículos" },
                {8  ," Caída de Cargas" },
                {9  ," Caída de Objetos" },
                {241," Caída del grupo electrógeno" },
                {162," Caída por desplome" },
                {10 ," Caídas a distinto nivel" },
                {11 ," Caídas al mismo nivel" },
                {200," Caídas al mismo y distinto nivel, así como de materiales al interior de la zanja" },
                {238," Caídas de objetos por manipulación" },
                {233," Caídas de personas a distinto nivel" },
                {232," Caídas de personas al mismo nivel" },
                {212," Caídas del personal, objeto y maquinaria tanto a mismo como distinto nivel" },
                {234," Caídas por objetos desprendidos por manipulación y desplome" },
                {12 ," Carga Física" },
                {13 ," Carga Mental" },
                {158," Choques contra objetos" },
                {103," Choques contra otro vehícul" },
                {21 ," Choques y golpes" },
                {183," Colisiones y vuelcos con vehículos" },
                {14 ," Condiciones ambientales del puesto de trabajo" },
                {15 ," Configuración del puesto de trabajo" },
                {16 ," Confinamiento/Asfixia" },
                {105," Consideraciones generales" },
                {17 ," Contactos Eléctricos" },
                {242," Contactos eléctricos indirectos" },
                {18 ," Contactos Químicos" },
                {19 ," Contactos Térmicos" },
                {20 ," Cortes" },
                {132," Cortes o pinchazos" },
                {217," Cortes producidos por distintas herramientas o utensilios de obra" },
                {22 ," Daños a terceros" },
                {187," Deslizamientos de tubos en pendientes o de forma lateral" },
                {272," desmembramiento sangriento" },
                {205," Desprendimientos de piedras al interior de la franja" },
                {223," Desprendimientos de tierras" },
                {201," Desprendimientos y derrumbes de las paredes de las zanjas" },
                {23 ," Desprendimientos, desplome y derrumbe" },
                {202," Deterioro de la maquinaria y de las herramientas (cables, bulones, eslingas, etc.)" },
                {66 ," Disconfort Térmico" },
                {67 ," Estrés Térmico" },
                {150," Estrés térmico por calor" },
                {149," Estrés térmico por frío" },
                {219," Explosión de mangueras de agua o aire" },
                {24 ," Explosiones" },
                {196," Exposición a humos y gases" },
                {239," Exposición a radiaciones" },
                {243," Exposición al ruido" },
                {130," Fatiga física" },
                {144," Fatiga postural" },
                {82 ," Fatiga Visual" },
               
            {25 ," Golpes" },
                {244," Golpes con la empuñadura" },
                {224," Golpes contra objetos" },
                {227," Golpes contra objetos móviles e inmóviles de la máquina" },
                {186," Golpes contra partes móviles. (Tubos y otros objetos)" },
                {184," Golpes o aprisionamientos con partes móviles de máquinas" },
                {220," Golpes por movimiento de mangueras de salida" },
                {199," Golpes por objetos y herramientas" },
                {203," Golpes y atrapamientos entre columnas de tubos, maquinaria y tubos" },
                {189," Golpes y contactos contra elementos móviles, inmóviles, objetos y herramientas" },
                {235," Golpes y contactos contra elementos móviles, inmóviles, objetos y/o herramientas" },
                {236," Golpes y contactos por elementos móviles" },
                {134," Golpes, Atrapamientos" },
                {181," Heridas con flora y fauna" },
                {26 ," Iluminación" },
                {211," Impactos del calibre" },
               
            //{266,"	in" },
                {27 ,"Incendios" },
                {104,"Incendios/ explosión" },
                {63 ,"Inhalación de productos químicos" },
                {215,"Inhalación de vapores metálicos" },
                {191,"Inhalación, contacto con sustancias peligrosas y ambientes pulverulentos" },
                {226,"Inhalación, ingestión y contactos con sustancias peligrosas" },
                {182,"Insolaciones, quemaduras por el sol" },
                {221,"Inundación o desprendimiento de una máquina al dejar las zonas inundables" },
                {210,"Latigazos de la manguera" },
                {112,"Manipulación de Cargas" },
                {197,"Manipulación de productos químicos (disolventes, pinturas etc.)" },
                {152,"MANIPULACIÓN DE SUSTANCIAS QUÍMICAS (espumógeno,grasas/aceites lubricantes, gases extintores- F13, N2, CO2, HFC 227, Ar, S3…-)"},
                {28  ,"Maquinaria automotriz y vehículos" },
                {69  ,"Movimientos Repetitivos" },
            
            {60  ,"Otros"},
                {249 ,"Otros (Campo eléctrico)" },
                {148 ,"Otros: Agresión física"},
                { 101,"Otros: ausencia de formación" },
                {29  ,"Pisadas" },
                {237 ,"Pisadas sobre objetos"},
                {230 ,"Polvo"},
                {70  ,"Posturas Forzadas"},
                {207 ,"Problemas de circulación interna por caminos en mal estado"},
                {225 ,"Proyección de esquirlas y salpicaduras de los materiales"},
                {177 ,"Proyección de fragmentos o partículas"},
                {216 ,"Proyección de partículas y heridas en los ojos por cuerpos extraños"},
                {30  ,"Proyecciones"},
                {194 ,"Quemaduras"},
                {193 ,"Radiación del arco eléctrico"},
                {214 ,"Radiaciones de la Soldadura"},
                {31  ,"Radiaciones Ionizantes"},
                {32  ,"Radiaciones no ionizantes"},
                {195 ,"Radiaciones ultravioletas y luminosas"},
                {218 ,"Riesgo de incendio por partículas incandescentes" },
                {62  ,"Riesgo no evaluable" },
                {246 ,"riesgo prueba" },
                {208 ,"Riesgos a terceros por intromisión controlada"},
                {204 ,"Riesgos derivados de condiciones meteorológicas adversas"},
                {222 ,"Riesgos derivados de tuberías y objetos a presión"},
                {33  ,"Ruido"},
                {209 ,"Ruido por la maquinaria"},
                {34  ,"Sobrecarga Térmica"},
                {35  ,"Sobreesfuerzos"},
                {36  ,"Ventilación"},
               
            {87  ,"Ver"},
            //{269 ,"ver"},
            //{270 ,"VER"},
                {126 ,"Ver “trabajos con máquina quitanieve”"},
                {180 ,"Ver capítulo de \"Construcción y rehabilitación\""  },
                {113 ,"Ver capítulo de trabajos de tala y poda"      },
                {271 ,"Ver capítulo: \"Pintura\"."                 },
                {110 ,"Ver Evaluación de Riesgos “Trabajos con maquinaria”: Palaretroexcavadora y camión basculante."},
                {128 ,"Ver pto “1.17.1”"},
                {151 ,"Ver punto 28.13.1"},
                {164 ,"Ver punto anterior"},
                {124 ,"Ver riegos “ Camión grúa”"},
                {252 ,"Ver riesgo en el capítulo de “Trabajos en centros de Transformación”"},
                {248 ,"Ver riesgos  \" Red de tierras\"."},
                {117 ,"Ver riesgos  “Rodillo compactador”"},
                {247 ,"Ver riesgos \" Conduciones de Gas\""},
                {93  ,"Ver riesgos \"apartado 8.2.6  (Tendido de cable/fibra óptica en fachada)\"."},
                {120 ,"Ver riesgos “ Camión extensión  bituminosa”"},
                {122 ,"Ver riesgos “ Máquina retroexcavadora”"},
                {116 ,"Ver riesgos “Camión riego bituminoso”"},
                {123 ,"Ver riesgos “Camión volquete”"},
                {121 ,"Ver riesgos “Tratamientos superficiales”. 1.27.1"},
                {173 ,"Ver riesgos apartado de Reposición de pavimento localizado"},
                {127 ,"Ver riesgos con \"Camión cuba de riego\""},
                {107 ,"Ver riesgos contenido en el capítulo \"Montaje y mantenimiento de instalaciones de alumbrado públco y semáforos\""},
                {146 ,"Ver riesgos de aplicación de fitosanitarios y biocidas."},
                {115 ,"Ver riesgos de trabajo con \"Barredora\""},
                {265 ,"Ver riesgos de trabajos de ahoyado y plantación por medios manuales"},
                {139 ,"Ver riesgos del apartado 10.2.1 - \"Obra civil y cimentaciones de alumbrado público\"."},
                {141 ,"Ver riesgos del apartado 10.2.14 - \"Instalación de cuadros eléctrico\"."},
                {142 ,"Ver riesgos del apartado 10.2.17 - \"Limpieza y pintura de farolas\"."},
                {140 ,"Ver riesgos del apartado 10.2.8 - \"Montaje e izado de farolas\"."},
              
             //{98  ,"Ver riesgos en "},
                {94  ,"Ver riesgos en \" apartado 8.2.10, empalme y conexiones de cable y fibra óptica en fachada\"."},
                {95  ,"Ver riesgos en \" apartado 8.2.15 (Desmontaje de instalaciones en fachada)\"."},
                {99  ,"Ver riesgos en \" Vibrador de hormigón con motor de explosión\""},
                {262 ,"Ver riesgos en \"Aplicaión de fitosanitarios y biocidas\""},
                {58  ,"Ver riesgos en \"Construcción y montaje de canalización de tubería\""},
                {254 ,"Ver riesgos en \"Mantenimiento de instalaciones frigoríficas y térmicas\"."},
                {253 ,"Ver riesgos en \"Mantenimiento higiénico sanitario\""},
                {147 ,"Ver riesgos en \"Maquinaria para elevación de cargas\""},
                {84  ,"Ver Riesgos en \"Red general de tierras\""},
                {83  ,"Ver riesgos en \"Trabajos con Carretilla Elevadora\""},
                {55  ,"Ver riesgos en \"Trabajos con Grúa Torre\""},
                {250 ,"Ver riesgos en \"Trabajos con Helicóptero\"."},
                {165 ,"Ver riesgos en \"Trabajos con maquinaria para la elevación de personas\"."} ,
                {240 ,"Ver riesgos en \"Trabajos con maquinaría\""},
                {133 ,"Ver riesgos en \"Trabajos con Maquinaria-Plataforma elevadora\""},
                {50  ,"Ver riesgos en \"Trabajos con máquinas y herramientas\""},
                {153 ,"Ver riesgos en \"Trabajos de acondicionamiento y mantenimiento de jardines\""},
                {57  ,"Ver riesgos en \"Trabajos de Albañilería\""},
                {38  ,"Ver riesgos en \"Trabajos de Excavación\""},
                {46  ,"Ver riesgos en \"Trabajos de Red de Tierras\""},
                {102 ,"Ver riesgos en \"Trabajos de Soldadura\""},
                {43  ,"Ver riesgos en \"Trabajos en C. T.\""},
                {54  ,"Ver riesgos en \"Trabajos en Cajas de Medida.....\""},
                {44  ,"Ver riesgos en \"Trabajos en conducciones de Gas\""},
                {45  ,"Ver riesgos en \"Trabajos en F.F. Catenaria\""},
                {92  ,"Ver riesgos en \"Trabajos en fachadas y Trabajos verticales del capítulo : Trabajos en Postes, Tejados y Fachadas\""},
                {40  ,"Ver riesgos en \"Trabajos en Montajes Industriales\""},
                {41  ,"Ver riesgos en \"Trabajos en Postes ,Tejados.....\""},
                {145 ,"Ver riesgos en \"Trabajos en subestaciones y estaciones receptoras\"."},
                {42  ,"Ver riesgos en \"Trabajos en Subestaciones......\""},
                {51  ,"Ver riesgos en \"Trabajos en Tensión\""},
                {100 ,"Ver riesgos en \"Vibrador de hormigón eléctrico\""},
                {97  ,"Ver riesgos en \"Alisadora con eléctrica\""},
                {88  ,"Ver riesgos en \"El uso de ahoyador ¨Trabajos con camión Grúa¨\"."},
                {85  ,"Ver riesgos en \"Trabajos con grúa Autopropulsada\""},
                {91  ,"Ver riesgos en \"Trabajos con Maquinaria: equipo de tendido\""},
                {90  ,"Ver riesgos en \"Trabajos con Plataforma Elevadora\""},
                {155 ,"Ver riesgos en “Trabajos en Postes, postecillos y fachadas”"},
                {154 ,"Ver riesgos en “Trabajos en tejados y cubiertas”"},
                {56  ,"Ver riesgos en \"Trabajos en Espacios Confinados\""},
                {176 ,"Ver riesgos en apartado de Retirada programada de desprendimientos"},
                {39  ,"Ver riesgos en capítulo \"Construcción y montaje de líneas de distribución y transporte de energía eléctrica\""},
                {53  ,"Ver riesgos en capítulo \"Edificación y rehabilitación\""},
                {49  ,"Ver riesgos en capítulo \"Maquinaria para elevación de cargas\""},
                {52  ,"Ver riesgos en capítulo \"Trabajos con escaleras y andamios\""},
                {111 ,"Ver riesgos en capítulo \"Trabajos con productos químicos\""},
                {179 ,"Ver riesgos en capítulo \"Trabajos de conservación de carreteras\""},
                {169 ,"Ver riesgos en capítulo de \"Maquinaria para elevación de cargas\""},
                {131 ,"Ver riesgos en capítulo de \"Trabajos con máquinas y herramientas\""},
                {166 ,"Ver riesgos en capítulo de \"Trabajos con máquinas y herramientas\""},
                {172 ,"Ver riesgos en capítulo de \"Trabajos con productos químicos\""},
                {168 ,"Ver riesgos en capítulo de \"Trabajos de excavación y hormigonado\""},
                {89  ,"Ver riesgos en capítulo de \"Trabajos de excavación y hormigonado\""},
                {167 ,"Ver riesgos en capítulo de \"Trabajos de manipulación de cargas\""},
                {174 ,"Ver riesgos en el apartado Tratamientos superficiales\""},
                {170 ,"Ver riesgos en el capítulo \"Manipulación de cargas\""},
                {48  ,"Ver riesgos en el capítulo \"Manipulación de cargas\""},
                {178 ,"Ver riesgos en el capítulo \"Mantenimiento de parques y jardines\""},
                {108 ,"Ver riesgos en el capítulo \"Montaje y mantenimiento de instalacciones de alumbrado público y semáforos\"."},
                {175 ,"Ver riesgos en el capítulo \"Trabajos con maquinaria\""},
                {47  ,"Ver riesgos en el capítulo \"Trabajos con maquinaria\""},
                {106 ,"Ver Riesgos en el capítulo \"Trabajos con maquinaria\""},
                {109 ,"Ver riesgos en el capítulo \"Trabajos con máquinas y herramientas\"."},
                {171 ,"Ver riesgos en el capítulo \"Trabajos de excavación y hormigonado\""},
                {163 ,"Ver riesgos en el capítulo “Trabajos con Máquinas y herramientas”"},
                {251 ,"Ver riesgos en la evaluación de \"Construscción, montaje y mantenimiento de líneas eléctricas\""},
                {129 ,"Ver riesgos evaluación de maquinaria “Barredora”"},
                {118 ,"Ver riesgos pto “1.26.2”"},
                {125 ,"Ver riesgos pto 1.28.1"},
                {135 ,"Ver riesgos punto 10.2.7 - \"Instalación de puntos de luz\"."},
                {143 ,"Ver riesgos punto 10.3.4 - \"Tendido de cable subterráneo y conexiones\"."},
                {137 ,"Ver riesgos puntos 10.2.(11 - 12 - 13) - \"Tendido de cable\"."},
                {119 ,"Ver riesgos trabajos \"gravilladora\""},
                {114 ,"Ver riesgos Trabajos con “Fresadora”"},
                {264 ,"Ver trabajos de carga y transporte con caballería"},
                {263 ,"Ver trabajos de limpieza con motodesbrozadora"},
                {37  ,"Vibraciones"},
             
                {161 ,"Vuelco"},
                {185 ,"Vuelco de grúas móviles y máquinas"},
                {81  ,"Vuelco de vehículos o maquinaria"},
        };
    }
}