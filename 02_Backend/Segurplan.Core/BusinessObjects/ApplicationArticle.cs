using System;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationArticle {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public decimal TimeOfWork { get; set; }
        public int MinimumUnit { get; set; }
        /// <summary>
        /// Preción BASE articulo
        /// </summary>
        public decimal Price { get; set; }
        public decimal AmortizationTime { get; set; }
        /// <summary>
        /// Precio de articulo por dia
        /// </summary>
        public decimal AmortizationPrice { get; set; }
        /// <summary>
        /// Precio de articulo por dia x duracion de la obra
        /// </summary>
        public decimal PriceDurationWork { get; set; }
        public int Unit { get; set; }
        /// <summary>
        /// Precio total del articulo por dia x cantidad unidades
        /// </summary>
        public decimal TotalPrice { get; set; }
        public int IdArticleFamily { get; set; }
        public string Number { get; set; }
        public string FamilyName { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
