using LiftManager.ViewModels;
using LiftManager.Model;
using LiftManager.Views;
using LiftManager.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using LiftManager.Util;

namespace LiftManager.Screens
{
    public class LiftSimulationScreen : ContentPage
    {
        Grid visualisationGrid;
        List<FloorView> floorViews;
        List<LiftView> liftViews;
        LiftSimulation viewModel;

        UpdateTimer _timer;
        UpdateTimer Timer
        {
            get
            {
                return _timer;
            }
            set
            {
                if (_timer != null)
                    _timer.disposed = true;

                _timer = value;
            }
        }

        public LiftSimulationScreen ()
        {
            viewModel = new LiftSimulation();

            viewModel.OnFloorUpdate += new EventHandler<FloorEventArgs>(OnFloorUpdate);
            viewModel.OnLiftUpdate += new EventHandler<LiftEventArgs>(OnLiftUpdate);

            PrepareSimulationVisualisation();

            StartUpdateLoop();
        }

        // ContentPage control display

        void PrepareSimulationVisualisation ()
        {
            visualisationGrid = CreateVisualisationGrid(viewModel.Floors.Count, viewModel.Lifts.Count);

            floorViews = new List<FloorView>();
            liftViews = new List<LiftView>();

            for (int i = 0; i < viewModel.Floors.Count; i++)
            {
                FloorView floorView = new FloorView(viewModel.Floors[i]);
                int gridRow = (visualisationGrid.RowDefinitions.Count) - i; // offset by 1 to account for header row

                visualisationGrid.Children.Add(floorView.view, 0, gridRow);
                floorView.CreatePartyClick += OnCreatePartyClick;
                floorViews.Add(floorView);                
            }

            for (int i = 0; i < viewModel.Lifts.Count; i++)
            {
                LiftView liftView = new LiftView(viewModel.Lifts[i], viewModel.Floors.Count);
                int column = liftView.data.liftNumber; // liftNumber is not a zero-based index, so it accounts for the 'floor' column offset

                for (var r = 1; r < (visualisationGrid.RowDefinitions.Count +1); r++)
                {
                    int gridRow = (visualisationGrid.RowDefinitions.Count + 1) - r; // offset by 1 to account for header row

                    Label rowContent = new Label
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Color.White
                    };

                    liftView.views.Add(rowContent);
                    visualisationGrid.Children.Add(rowContent, column, gridRow);
                }

                liftView.DrawView();
                liftViews.Add(liftView);
            }

            Content = new ScrollView
            {
                Content = visualisationGrid,
                BackgroundColor = Color.FromHex("#333333")
            };
        }

        Grid CreateVisualisationGrid (int numberOfFloors, int numberOfLifts)
        {
            int rows = numberOfFloors;
            int columns = numberOfLifts + 1; // extra column to account for the floor column 

            Grid grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = 0
            };

            for (int r = 0; r < rows; r++)
                grid.RowDefinitions.Add(new RowDefinition());

            for (int c = 0; c < columns; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = (c > 0) ? new GridLength(1, GridUnitType.Star) : new GridLength(2, GridUnitType.Star)
                });
            }

            return grid;
        }

        // ViewModel observations

        void OnFloorUpdate (object sender, FloorEventArgs args)
        {
            GetFloorView(args.floor).viewIsDirty = true;
        }

        void OnLiftUpdate (object sender, LiftEventArgs args)
        {
            GetLiftView(args.lift).viewIsDirty = true;
        }

        void OnPartyCreateConfirmation(object sender, PartyEventArgs args)
        {
            viewModel.CreateTravelParty(args);
            StartUpdateLoop();
        }

        void OnPartyCreateCancel(object sender, EventArgs args)
        {
            StartUpdateLoop();
        }

        // Button events

        async void OnCreatePartyClick(object sender, FloorEventArgs args)
        {
            var nextPage = new CreatePartyScreen(
                args.floor, 
                OnPartyCreateConfirmation, 
                OnPartyCreateCancel
            );
            StopUpdateLoop();
            await Navigation.PushModalAsync(nextPage);
        }

        // Update loop

        void StartUpdateLoop ()
        {
            Timer = new UpdateTimer(500, OnUpdate);
        }

        void StopUpdateLoop ()
        {
            Timer = null;
        }

        void OnUpdate (object sender, UpdateTimer args)
        {
            if (!args.disposed)
            {
                viewModel.StepSimulation();
            }

            foreach (FloorView floorView in floorViews)
            {
                if (floorView.viewIsDirty)
                    floorView.DrawView();
            }

            foreach (LiftView liftView in liftViews)
            {
                if (liftView.viewIsDirty)
                    liftView.DrawView();
            }
        }

        // Visualisation Helpers

        FloorView GetFloorView (Floor floor)
        {
            foreach (FloorView fv in floorViews)
            {
                if (fv.data == floor)
                    return fv;
            }

            return null;   
        }

        LiftView GetLiftView (Lift lift)
        {
            foreach (LiftView liftView in liftViews)
            {
                if (liftView.data == lift)
                    return liftView;
            }

            return null;
        }
    }
}
