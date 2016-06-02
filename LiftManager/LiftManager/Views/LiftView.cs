using LiftManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.Views
{
    /// <summary>
    /// View that uses Xamarin.Forms controls to represent Lift model data.
    /// </summary>
    public class LiftView
    {
        public Lift data;
        public List<Label> views = new List<Label>();
        public bool viewIsDirty;

        public LiftView (Lift lift, int numberOfFloors)
        {
            data = lift;
        }

        public void DrawView ()
        {
            /*
                Lift View consists of a column of empty 'cells' and one that displays the 
                lift's current occupancy and travel direction.
            */

            for (int i = 1; i <= views.Count; i++)
            {
                if (data.currentLevel == i)
                {
                    SetCarriageLabelView(views[i - 1]);
                } else
                {
                    SetEmptyView(views[i - 1]);
                }                
            }
            viewIsDirty = false;
        }

        void SetEmptyView (Label view)
        {
            view.Text = "";
            view.BackgroundColor = Color.Transparent;
        }

        void SetCarriageLabelView (Label view)
        {
            string carriageStatusSymbol = "-";

            if (data.destinationLevel > 0)
            {
                if (data.destinationLevel > data.currentLevel)
                    carriageStatusSymbol = "^";
                else if (data.destinationLevel < data.currentLevel)
                    carriageStatusSymbol = "v";
            }

            view.Text = String.Format("{0} {1} {2}", data.Name + System.Environment.NewLine, data.passengers, carriageStatusSymbol);
            view.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            view.BackgroundColor = (data.CurrentStatus == Lift.Status.Idle) ? Color.FromHex("#cb605a") : Color.FromHex("#139549");
        }

        Label GetLiftCarriageView ()
        {
            string carriageStatusSymbol = "-";

            if (data.destinationLevel > 0)
            {
                if (data.destinationLevel > data.currentLevel)
                    carriageStatusSymbol = "^";
                else if (data.destinationLevel < data.currentLevel)
                    carriageStatusSymbol = "v";
            }

            Label carriageView = new Label
            {
                Text = String.Format("{0} {1} {2}", data.Name + System.Environment.NewLine, data.passengers, carriageStatusSymbol),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                BackgroundColor = (data.CurrentStatus == Lift.Status.Idle) ? Color.FromHex("#cb605a") : Color.FromHex("#139549")
            };

            return carriageView;
        }
    }
}
