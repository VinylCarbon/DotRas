//--------------------------------------------------------------------------
// <copyright file="RasComponent.cs" company="Jeff Winn">
//      Copyright (c) Jeff Winn. All rights reserved.
//
//      The use and distribution terms for this software is covered by the
//      GNU Library General Public License (LGPL) v2.1 which can be found
//      in the License.rtf at the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by
//      the terms of this license.
//
//      You must not remove this notice, or any other, from this software.
// </copyright>
//--------------------------------------------------------------------------

namespace DotRas.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides the base implementation for remote access service (RAS) components. This class must be inherited. 
    /// </summary>
    [ToolboxItem(false)]
    public abstract class RasComponent : Component
    {
        #region Fields

        private ISynchronizeInvoke _synchronizingObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.Design.RasComponent"/> class.
        /// </summary>
        protected RasComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotRas.Design.RasComponent"/> class.
        /// </summary>
        /// <param name="container">An <see cref="System.ComponentModel.IContainer"/> that will contain this component.</param>
        protected RasComponent(IContainer container)
        {
            if (container != null)
            {
                container.Add(this);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls that are issued by the component.
        /// </summary>
        [DefaultValue(null)]
        [SRDescription("RCSyncObjectDesc")]
        public ISynchronizeInvoke SynchronizingObject
        {
            get { return this._synchronizingObject; }
            set { this._synchronizingObject = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected virtual void InitializeComponent()
        {
        }

        /// <summary>
        /// Raises the event specified by <paramref name="method"/> with the event data provided. 
        /// </summary>
        /// <typeparam name="TEventArgs">The <see cref="System.EventArgs"/> used by the event delegate.</typeparam>
        /// <param name="method">The event delegate being raised.</param>
        /// <param name="e">An <typeparamref name="TEventArgs"/> containing event data.</param>
        protected void RaiseEvent<TEventArgs>(EventHandler<TEventArgs> method, TEventArgs e) where TEventArgs : EventArgs
        {
            if (method != null && this.CanRaiseEvents)
            {
                lock (method)
                {
                    if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
                    {
                        this.SynchronizingObject.Invoke(method, new object[] { this, e });
                    }
                    else
                    {
                        method(this, e);
                    }
                }
            }
        }

        #endregion
    }
}