using Model.Competition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.GeneralList
{
    class GeneralListItemModel
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }

        public static implicit operator GeneralListItemModel(CompetitionInfoModel model)
        {
            return new GeneralListItemModel()
            {
                ItemID = model.CompetitionID,
                ItemName = model.CompetitionName
            };
        }
    }
}
