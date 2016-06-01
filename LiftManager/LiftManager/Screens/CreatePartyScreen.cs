using LiftManager.Model;
using LiftManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace LiftManager.Screens
{
    public class CreatePartyScreen : ContentPage
    {
        public event EventHandler<PartyEventArgs> OnConfirm;
        public event EventHandler<EventArgs> OnCancel;

        Floor floor;
        Stepper destinationStepper, passengerStepper;
        Label destinationLabel, passengersLabel;
        Button confirmButton, cancelButton;

        public CreatePartyScreen(Floor floor, EventHandler<PartyEventArgs> ConfirmEvent, EventHandler<EventArgs> CancelEvent)
        {
            this.floor = floor;

            OnConfirm = ConfirmEvent;
            OnCancel = CancelEvent;

            DrawScreen(floor);
        }

        public void DrawScreen (Floor floor)
        {
            Label titleLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = String.Format("Floor {0} : Create Travel Party", floor.floorNumber)
            };

            Label descriptionLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Please select the number of passengers and desired floor for this journey."
            };

            destinationStepper = new Stepper
            {
                Value = floor.floorNumber,
                Minimum = 1,
                Maximum = 10,
                Increment = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            destinationStepper.ValueChanged += OnDestinationStepperValueChanged;

            passengerStepper = new Stepper
            {
                Value = 0,
                Minimum = 0,
                Maximum = floor.occupants,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            passengerStepper.ValueChanged += OnPassengersStepperValueChanged;

            destinationLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = String.Format("Destination floor : {0}/{1}", (int)destinationStepper.Value, 10)
            };

            passengersLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = String.Format("Passengers travelling : {0}/{1} ", (int)passengerStepper.Value, floor.occupants)
            };

            cancelButton = new Button { Text = "Cancel" };
            cancelButton.Clicked += OnCancelButtonClicked;

            confirmButton = new Button { Text = "Confirm" };
            confirmButton.Clicked += OnConfirmButtonClicked;
            confirmButton.IsEnabled = (destinationStepper.Value != floor.floorNumber);

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    titleLabel,
                    descriptionLabel,
                    passengersLabel,
                    passengerStepper,
                    destinationLabel,
                    destinationStepper,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Children = {
                            cancelButton,
                            confirmButton
                        }
                    }
                }
            };
        }

        async void OnCancelButtonClicked (object sender, EventArgs args)
        {
            OnCancel(this, args);
            await Navigation.PopModalAsync(true);
        }

        async void OnConfirmButtonClicked (object sender, EventArgs args)
        {
            OnConfirm(this, new PartyEventArgs(floor, (int)passengerStepper.Value, (int)destinationStepper.Value));
            await Navigation.PopModalAsync(true);
        }

        void OnPassengersStepperValueChanged (object sender, ValueChangedEventArgs e)
        {
            passengersLabel.Text = String.Format("Passengers travelling : {0}/{1} ", (int)e.NewValue, floor.occupants);
        }

        void OnDestinationStepperValueChanged(object sender, ValueChangedEventArgs e)
        {                        
            destinationLabel.Text = String.Format("Destination Floor : {0}/{1}", (int)e.NewValue, 10);

            // Disable 'confirm' button if the destination is set to the current floor
            confirmButton.IsEnabled = (e.NewValue != floor.floorNumber);                
        }
    }
}
