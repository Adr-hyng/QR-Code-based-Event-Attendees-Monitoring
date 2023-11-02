﻿using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class ProfileScreenViewModel: ViewModelBase
    {
        public Dictionary<string, RadioButtonViewModel> RadioButtons { get; set; }

        private const bool AUTO_CLOSE = false;
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _membership;
        public string Membership
        {
            get
            {
                return _membership;
            }
            set
            {
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }
        private string _position;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private string _institution;
        public string Institution
        {
            get
            {
                return _institution;
            }
            set
            {
                _institution = value;
                OnPropertyChanged(nameof(Institution));
            }
        }

        public ICommand StayCommand { get; private set; }
        public ICommand GoBackCommand { get;}
        public ICommand ToggleCommand { get;}

        public ProfileScreenViewModel(NavigationService GoToIdleScreen)
        {
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
            ToggleCommand = new ToggleButtonCommand(RadioButtons!);
        }

        public ProfileScreenViewModel(NavigationService GoToIdleScreen, Attendee profile)
        {
            FirstName = profile.fn;
            Name = $"{profile.fn} {(!String.IsNullOrEmpty(profile.mi) ? profile.mi + "." : "")} {profile.ln}";
            Membership = profile.membership;
            Position = profile.position;
            Institution = profile.institution;
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
            ToggleCommand = new ToggleButtonCommand(RadioButtons!); // Soon for Developer Mode To Manually Toggle
            RadioButtonsHandler(profile);

            if (AUTO_CLOSE) CloseTimerOption(Seconds: 3);
        }

        public void RadioButtonsHandler(Attendee profile)
        {
            Dictionary<string, bool> dayContents = new Dictionary<string, bool>()
            {
                ["AMD1"] = profile.day1.amSnack.HasValue,
                ["LD1"] = profile.day1.lunchSnack.HasValue,
                ["PMD1"] = profile.day1.pmSnack.HasValue,
                ["CheckInD1"] = profile.day1.checkIn.HasValue,
                ["CheckOutD1"] = profile.day1.checkOut.HasValue,

                ["AMD2"] = profile.day2.amSnack.HasValue,
                ["LD2"] = profile.day2.lunchSnack.HasValue,
                ["PMD2"] = profile.day2.pmSnack.HasValue,
                ["CheckInD2"] = profile.day2.checkIn.HasValue,
                ["CheckOutD2"] = profile.day2.checkOut.HasValue,

                ["AMD3"] = profile.day3.amSnack.HasValue,
                ["LD3"] = profile.day3.lunchSnack.HasValue,
                ["PMD3"] = profile.day3.pmSnack.HasValue,
                ["CheckInD3"] = profile.day3.checkIn.HasValue,
                ["CheckOutD3"] = profile.day3.checkOut.HasValue,
            };

            RadioButtons = new Dictionary<string, RadioButtonViewModel>();

            foreach (KeyValuePair<string, bool> ToggleButtonContent in dayContents)
            {
                RadioButtons.Add(ToggleButtonContent.Key, new RadioButtonViewModel { IsChecked = true, Opacity = ToggleButtonContent.Value ? 1 : 0 });
            }
        }

        public async void CloseTimerOption(int Seconds)
        {
            await Task.Delay(1000 * Seconds);
            GoBackCommand.Execute(null);
        }
    }
}
