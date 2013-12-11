/*
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace Affine.Dnn.Modules.ATI_Routes
{   
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Info class for the ATI_Register
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_RoutesInfo
    {

    #region Private Members

        private int _ModuleId;
        private int _ItemId;
        private string _Content;
        private int _CreatedByUser;
        private DateTime _CreatedDate;

    #endregion

    #region Constructors

        // initialization
        public ATI_RoutesInfo()
        {
        }

    #endregion

    #region Public Properties

        /// <summary>
        /// Gets and sets the Module Id
        /// </summary>
        public int ModuleId
        {
            get
            {
                return _ModuleId;
            }
            set
            {
                _ModuleId = value;
            }
        }

        /// <summary>
        /// Gets and sets the Item Id
        /// </summary>
        public int ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                _ItemId = value;
            }
        }

        /// <summary>
        /// gets and sets the content
        /// </summary>
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }

        /// <summary>
        /// Gets and sets the User Id who Created/Updated the content
        /// </summary>
        public int CreatedByUser
        {
            get
            {
                return _CreatedByUser;
            }
            set
            {
                _CreatedByUser = value;
            }
        }

        /// <summary>
        /// Gets and sets the Date when Created/Updated
        /// </summary>
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                _CreatedDate = value;
            }
        }


    #endregion
    
    }
}
