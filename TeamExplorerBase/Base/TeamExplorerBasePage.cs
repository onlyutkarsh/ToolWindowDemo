﻿/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/

using System;
using Microsoft.TeamFoundation.Controls;

namespace TeamExplorerIntegration.Base
{
    /// <summary>
    /// Team Explorer page base class.
    /// </summary>
    public class TeamExplorerBasePage : TeamExplorerBase, ITeamExplorerPage
    {
        #region ITeamExplorerPage

        private bool _isBusy;

        private object _pageContent;

        private string _title;

        /// <summary>
        /// Get/set the IsBusy flag.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        /// <summary>
        /// Get/set the page content.
        /// </summary>
        public object PageContent
        {
            get { return _pageContent; }
            set { _pageContent = value; RaisePropertyChanged("PageContent"); }
        }

        /// <summary>
        /// Get/set the page title.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }

        /// <summary>
        /// Cancel any running operations.
        /// </summary>
        public virtual void Cancel()
        {
        }

        /// <summary>
        /// Get the requested extensibility service from the page.  Return
        /// null if the service is not offered by this page.
        /// </summary>
        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        public virtual void Initialize(object sender, PageInitializeEventArgs e)
        {
            ServiceProvider = e.ServiceProvider;
        }

        /// <summary>
        /// Loaded handler that is called once the page and all sections
        /// have been initialized.
        /// </summary>
        public virtual void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        /// <summary>
        /// Refresh the page contents.
        /// </summary>
        public virtual void Refresh()
        {
        }

        /// <summary>
        /// Save context handler that is called before a page is unloaded.
        /// </summary>
        public virtual void SaveContext(object sender, PageSaveContextEventArgs e)
        {
        }

        #endregion ITeamExplorerPage
    }
}