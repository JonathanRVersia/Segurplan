using System;
using System.Collections.Generic;
using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Core.Extensions.Comparers {
    public class PlanSubChapterComparer : IEqualityComparer<PlanSubChapter> {

        public bool Equals(PlanSubChapter a, PlanSubChapter b) => a.Id == b.Id;

        public int GetHashCode(PlanSubChapter ch) {

            //Check whether the object is null
            if (Object.ReferenceEquals(ch, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashTitle = ch.Title == null ? 0 : ch.Title.GetHashCode();

            //Get hash code for the Code field.
            int hashId = ch.Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashTitle ^ hashId;
        }
    }
}
