using LiftManager.Model;
using LiftManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.Views
{
    /// <summary>
    /// View that uses Xamarin.Forms controls to represent Floor model data.
    /// </summary>
    public class FloorView
    {
        public Floor data;
        public ContentView view;
        public event EventHandler<FloorEventArgs> CreatePartyClick;
        public bool viewIsDirty;

        public FloorView (Floor floor)
        {
            data = floor;
            DrawView();
        }

        public void DrawView ()
        {
            /*
                Floor View consists of a floor number, an occupants counter and a 
                column that shows either waiting travellers or a button to create a travel party. 
            */

            if (view == null)
            {
                view = new ContentView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Content = null
                };
            }

            StackLayout containerLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#6bc0c4"),
                Padding = 0,
                Spacing = 0
            };
            
            Label floorTitleLabel = new Label
            {
                Text = String.Format("Floor {0}", data.floorNumber),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#0b78bb")
            };
            containerLayout.Children.Add(floorTitleLabel);

            StackLayout contentLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                BackgroundColor = Color.FromHex("#6bc0c4"),
                Spacing = 0
            };
            containerLayout.Children.Add(contentLayout);

            StackLayout occupantContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = 0,
                Children = {
                    new Image {
                        Aspect = Aspect.AspectFill,
                        WidthRequest = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        Source = ImageSource.FromResource("LiftManager.meeple.png"),
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    },
                    new Label
                    {
                        Text = data.occupants.ToString(),
                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                }
            };
            contentLayout.Children.Add (occupantContainer);
                                                
            if (data.travellers > 0)
            {
                contentLayout.Children.Add (GetWaitingPartyView ());    
            } else if (data.occupants > 0)
            {
                contentLayout.Children.Add (GetCreatePartyLabelView ());
            }

            view.Content = containerLayout;
            viewIsDirty = false;
        }

        Label GetWaitingPartyView ()
        {
            Label waitingPartyView = new Label
            {
                Text = String.Format("{0} ({1}){2}", data.travellers, data.travelDestination, (data.travelDestination < data.floorNumber) ? "v" : "^"),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof (Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#e76a5e")
            };

            return waitingPartyView;
        }

        Label GetCreatePartyLabelView ()
        {
            // Using a label with a gesturerecognizer for more consistent rendering than a Forms Button

            Label createPartyLabel = new Label
            {
                Text = "+",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#1691ca")
            };

            createPartyLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command( () => OnActionSheetSimpleClicked() )
            });

            return createPartyLabel;
        }

        void OnActionSheetSimpleClicked()
        {
            CreatePartyClick(this, new FloorEventArgs(data));
        }
    }
}
