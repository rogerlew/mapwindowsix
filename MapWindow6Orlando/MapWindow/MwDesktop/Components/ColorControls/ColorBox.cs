//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/20/2008 10:54:05 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MapWindow;
using MapWindow.Main;
using MapWindow.Data;
using MapWindow.Drawing;
using MapWindow.Geometries;
using System.Windows.Forms;
using System.ComponentModel;
namespace MapWindow.Components
{


    /// <summary>
    /// ColorBox
    /// </summary>
    [DefaultEvent("SelectedItemChanged"), DefaultProperty("Value"),
    ToolboxBitmap(typeof(ColorBox), "UserControl.ico")]
    public class ColorBox : UserControl
    {

        #region Events

        /// <summary>
        /// Occurs when the selected color has been changed in the drop-down
        /// </summary>
        public event EventHandler SelectedItemChanged;


        #endregion

        private Label lblColor;
        private ColorDropDown cddColor;
        private Button cmdShowDialog;
  
       
        #region Private Variables

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorBox
        /// </summary>
        public ColorBox()
        {
            InitializeComponent();
            cddColor.SelectedIndexChanged += new EventHandler(cddColor_SelectedIndexChanged);
        }

        void cddColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, new EventArgs());
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorBox));
            this.lblColor = new System.Windows.Forms.Label();
            this.cddColor = new MapWindow.Components.ColorDropDown();
            this.cmdShowDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblColor
            // 
            this.lblColor.AccessibleDescription = null;
            this.lblColor.AccessibleName = null;
            resources.ApplyResources(this.lblColor, "lblColor");
            this.lblColor.Font = null;
            this.lblColor.Name = "lblColor";
            // 
            // cddColor
            // 
            this.cddColor.AccessibleDescription = null;
            this.cddColor.AccessibleName = null;
            resources.ApplyResources(this.cddColor, "cddColor");
            this.cddColor.BackgroundImage = null;
            this.cddColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cddColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cddColor.Font = null;
            this.cddColor.FormattingEnabled = true;
            this.cddColor.Items.AddRange(new object[] {
            ((object)(resources.GetObject("cddColor.Items"))),
            ((object)(resources.GetObject("cddColor.Items1"))),
            ((object)(resources.GetObject("cddColor.Items2"))),
            ((object)(resources.GetObject("cddColor.Items3"))),
            ((object)(resources.GetObject("cddColor.Items4"))),
            ((object)(resources.GetObject("cddColor.Items5"))),
            ((object)(resources.GetObject("cddColor.Items6"))),
            ((object)(resources.GetObject("cddColor.Items7"))),
            ((object)(resources.GetObject("cddColor.Items8"))),
            ((object)(resources.GetObject("cddColor.Items9"))),
            ((object)(resources.GetObject("cddColor.Items10"))),
            ((object)(resources.GetObject("cddColor.Items11"))),
            ((object)(resources.GetObject("cddColor.Items12"))),
            ((object)(resources.GetObject("cddColor.Items13"))),
            ((object)(resources.GetObject("cddColor.Items14"))),
            ((object)(resources.GetObject("cddColor.Items15"))),
            ((object)(resources.GetObject("cddColor.Items16"))),
            ((object)(resources.GetObject("cddColor.Items17"))),
            ((object)(resources.GetObject("cddColor.Items18"))),
            ((object)(resources.GetObject("cddColor.Items19"))),
            ((object)(resources.GetObject("cddColor.Items20"))),
            ((object)(resources.GetObject("cddColor.Items21"))),
            ((object)(resources.GetObject("cddColor.Items22"))),
            ((object)(resources.GetObject("cddColor.Items23"))),
            ((object)(resources.GetObject("cddColor.Items24"))),
            ((object)(resources.GetObject("cddColor.Items25"))),
            ((object)(resources.GetObject("cddColor.Items26"))),
            ((object)(resources.GetObject("cddColor.Items27"))),
            ((object)(resources.GetObject("cddColor.Items28"))),
            ((object)(resources.GetObject("cddColor.Items29"))),
            ((object)(resources.GetObject("cddColor.Items30"))),
            ((object)(resources.GetObject("cddColor.Items31"))),
            ((object)(resources.GetObject("cddColor.Items32"))),
            ((object)(resources.GetObject("cddColor.Items33"))),
            ((object)(resources.GetObject("cddColor.Items34"))),
            ((object)(resources.GetObject("cddColor.Items35"))),
            ((object)(resources.GetObject("cddColor.Items36"))),
            ((object)(resources.GetObject("cddColor.Items37"))),
            ((object)(resources.GetObject("cddColor.Items38"))),
            ((object)(resources.GetObject("cddColor.Items39"))),
            ((object)(resources.GetObject("cddColor.Items40"))),
            ((object)(resources.GetObject("cddColor.Items41"))),
            ((object)(resources.GetObject("cddColor.Items42"))),
            ((object)(resources.GetObject("cddColor.Items43"))),
            ((object)(resources.GetObject("cddColor.Items44"))),
            ((object)(resources.GetObject("cddColor.Items45"))),
            ((object)(resources.GetObject("cddColor.Items46"))),
            ((object)(resources.GetObject("cddColor.Items47"))),
            ((object)(resources.GetObject("cddColor.Items48"))),
            ((object)(resources.GetObject("cddColor.Items49"))),
            ((object)(resources.GetObject("cddColor.Items50"))),
            ((object)(resources.GetObject("cddColor.Items51"))),
            ((object)(resources.GetObject("cddColor.Items52"))),
            ((object)(resources.GetObject("cddColor.Items53"))),
            ((object)(resources.GetObject("cddColor.Items54"))),
            ((object)(resources.GetObject("cddColor.Items55"))),
            ((object)(resources.GetObject("cddColor.Items56"))),
            ((object)(resources.GetObject("cddColor.Items57"))),
            ((object)(resources.GetObject("cddColor.Items58"))),
            ((object)(resources.GetObject("cddColor.Items59"))),
            ((object)(resources.GetObject("cddColor.Items60"))),
            ((object)(resources.GetObject("cddColor.Items61"))),
            ((object)(resources.GetObject("cddColor.Items62"))),
            ((object)(resources.GetObject("cddColor.Items63"))),
            ((object)(resources.GetObject("cddColor.Items64"))),
            ((object)(resources.GetObject("cddColor.Items65"))),
            ((object)(resources.GetObject("cddColor.Items66"))),
            ((object)(resources.GetObject("cddColor.Items67"))),
            ((object)(resources.GetObject("cddColor.Items68"))),
            ((object)(resources.GetObject("cddColor.Items69"))),
            ((object)(resources.GetObject("cddColor.Items70"))),
            ((object)(resources.GetObject("cddColor.Items71"))),
            ((object)(resources.GetObject("cddColor.Items72"))),
            ((object)(resources.GetObject("cddColor.Items73"))),
            ((object)(resources.GetObject("cddColor.Items74"))),
            ((object)(resources.GetObject("cddColor.Items75"))),
            ((object)(resources.GetObject("cddColor.Items76"))),
            ((object)(resources.GetObject("cddColor.Items77"))),
            ((object)(resources.GetObject("cddColor.Items78"))),
            ((object)(resources.GetObject("cddColor.Items79"))),
            ((object)(resources.GetObject("cddColor.Items80"))),
            ((object)(resources.GetObject("cddColor.Items81"))),
            ((object)(resources.GetObject("cddColor.Items82"))),
            ((object)(resources.GetObject("cddColor.Items83"))),
            ((object)(resources.GetObject("cddColor.Items84"))),
            ((object)(resources.GetObject("cddColor.Items85"))),
            ((object)(resources.GetObject("cddColor.Items86"))),
            ((object)(resources.GetObject("cddColor.Items87"))),
            ((object)(resources.GetObject("cddColor.Items88"))),
            ((object)(resources.GetObject("cddColor.Items89"))),
            ((object)(resources.GetObject("cddColor.Items90"))),
            ((object)(resources.GetObject("cddColor.Items91"))),
            ((object)(resources.GetObject("cddColor.Items92"))),
            ((object)(resources.GetObject("cddColor.Items93"))),
            ((object)(resources.GetObject("cddColor.Items94"))),
            ((object)(resources.GetObject("cddColor.Items95"))),
            ((object)(resources.GetObject("cddColor.Items96"))),
            ((object)(resources.GetObject("cddColor.Items97"))),
            ((object)(resources.GetObject("cddColor.Items98"))),
            ((object)(resources.GetObject("cddColor.Items99"))),
            ((object)(resources.GetObject("cddColor.Items100"))),
            ((object)(resources.GetObject("cddColor.Items101"))),
            ((object)(resources.GetObject("cddColor.Items102"))),
            ((object)(resources.GetObject("cddColor.Items103"))),
            ((object)(resources.GetObject("cddColor.Items104"))),
            ((object)(resources.GetObject("cddColor.Items105"))),
            ((object)(resources.GetObject("cddColor.Items106"))),
            ((object)(resources.GetObject("cddColor.Items107"))),
            ((object)(resources.GetObject("cddColor.Items108"))),
            ((object)(resources.GetObject("cddColor.Items109"))),
            ((object)(resources.GetObject("cddColor.Items110"))),
            ((object)(resources.GetObject("cddColor.Items111"))),
            ((object)(resources.GetObject("cddColor.Items112"))),
            ((object)(resources.GetObject("cddColor.Items113"))),
            ((object)(resources.GetObject("cddColor.Items114"))),
            ((object)(resources.GetObject("cddColor.Items115"))),
            ((object)(resources.GetObject("cddColor.Items116"))),
            ((object)(resources.GetObject("cddColor.Items117"))),
            ((object)(resources.GetObject("cddColor.Items118"))),
            ((object)(resources.GetObject("cddColor.Items119"))),
            ((object)(resources.GetObject("cddColor.Items120"))),
            ((object)(resources.GetObject("cddColor.Items121"))),
            ((object)(resources.GetObject("cddColor.Items122"))),
            ((object)(resources.GetObject("cddColor.Items123"))),
            ((object)(resources.GetObject("cddColor.Items124"))),
            ((object)(resources.GetObject("cddColor.Items125"))),
            ((object)(resources.GetObject("cddColor.Items126"))),
            ((object)(resources.GetObject("cddColor.Items127"))),
            ((object)(resources.GetObject("cddColor.Items128"))),
            ((object)(resources.GetObject("cddColor.Items129"))),
            ((object)(resources.GetObject("cddColor.Items130"))),
            ((object)(resources.GetObject("cddColor.Items131"))),
            ((object)(resources.GetObject("cddColor.Items132"))),
            ((object)(resources.GetObject("cddColor.Items133"))),
            ((object)(resources.GetObject("cddColor.Items134"))),
            ((object)(resources.GetObject("cddColor.Items135"))),
            ((object)(resources.GetObject("cddColor.Items136"))),
            ((object)(resources.GetObject("cddColor.Items137"))),
            ((object)(resources.GetObject("cddColor.Items138"))),
            ((object)(resources.GetObject("cddColor.Items139"))),
            ((object)(resources.GetObject("cddColor.Items140"))),
            ((object)(resources.GetObject("cddColor.Items141"))),
            ((object)(resources.GetObject("cddColor.Items142"))),
            ((object)(resources.GetObject("cddColor.Items143"))),
            ((object)(resources.GetObject("cddColor.Items144"))),
            ((object)(resources.GetObject("cddColor.Items145"))),
            ((object)(resources.GetObject("cddColor.Items146"))),
            ((object)(resources.GetObject("cddColor.Items147"))),
            ((object)(resources.GetObject("cddColor.Items148"))),
            ((object)(resources.GetObject("cddColor.Items149"))),
            ((object)(resources.GetObject("cddColor.Items150"))),
            ((object)(resources.GetObject("cddColor.Items151"))),
            ((object)(resources.GetObject("cddColor.Items152"))),
            ((object)(resources.GetObject("cddColor.Items153"))),
            ((object)(resources.GetObject("cddColor.Items154"))),
            ((object)(resources.GetObject("cddColor.Items155"))),
            ((object)(resources.GetObject("cddColor.Items156"))),
            ((object)(resources.GetObject("cddColor.Items157"))),
            ((object)(resources.GetObject("cddColor.Items158"))),
            ((object)(resources.GetObject("cddColor.Items159"))),
            ((object)(resources.GetObject("cddColor.Items160"))),
            ((object)(resources.GetObject("cddColor.Items161"))),
            ((object)(resources.GetObject("cddColor.Items162"))),
            ((object)(resources.GetObject("cddColor.Items163"))),
            ((object)(resources.GetObject("cddColor.Items164"))),
            ((object)(resources.GetObject("cddColor.Items165"))),
            ((object)(resources.GetObject("cddColor.Items166"))),
            ((object)(resources.GetObject("cddColor.Items167"))),
            ((object)(resources.GetObject("cddColor.Items168"))),
            ((object)(resources.GetObject("cddColor.Items169"))),
            ((object)(resources.GetObject("cddColor.Items170"))),
            ((object)(resources.GetObject("cddColor.Items171"))),
            ((object)(resources.GetObject("cddColor.Items172"))),
            ((object)(resources.GetObject("cddColor.Items173"))),
            ((object)(resources.GetObject("cddColor.Items174"))),
            ((object)(resources.GetObject("cddColor.Items175"))),
            ((object)(resources.GetObject("cddColor.Items176"))),
            ((object)(resources.GetObject("cddColor.Items177"))),
            ((object)(resources.GetObject("cddColor.Items178"))),
            ((object)(resources.GetObject("cddColor.Items179"))),
            ((object)(resources.GetObject("cddColor.Items180"))),
            ((object)(resources.GetObject("cddColor.Items181"))),
            ((object)(resources.GetObject("cddColor.Items182"))),
            ((object)(resources.GetObject("cddColor.Items183"))),
            ((object)(resources.GetObject("cddColor.Items184"))),
            ((object)(resources.GetObject("cddColor.Items185"))),
            ((object)(resources.GetObject("cddColor.Items186"))),
            ((object)(resources.GetObject("cddColor.Items187"))),
            ((object)(resources.GetObject("cddColor.Items188"))),
            ((object)(resources.GetObject("cddColor.Items189"))),
            ((object)(resources.GetObject("cddColor.Items190"))),
            ((object)(resources.GetObject("cddColor.Items191"))),
            ((object)(resources.GetObject("cddColor.Items192"))),
            ((object)(resources.GetObject("cddColor.Items193"))),
            ((object)(resources.GetObject("cddColor.Items194"))),
            ((object)(resources.GetObject("cddColor.Items195"))),
            ((object)(resources.GetObject("cddColor.Items196"))),
            ((object)(resources.GetObject("cddColor.Items197"))),
            ((object)(resources.GetObject("cddColor.Items198"))),
            ((object)(resources.GetObject("cddColor.Items199"))),
            ((object)(resources.GetObject("cddColor.Items200"))),
            ((object)(resources.GetObject("cddColor.Items201"))),
            ((object)(resources.GetObject("cddColor.Items202"))),
            ((object)(resources.GetObject("cddColor.Items203"))),
            ((object)(resources.GetObject("cddColor.Items204"))),
            ((object)(resources.GetObject("cddColor.Items205"))),
            ((object)(resources.GetObject("cddColor.Items206"))),
            ((object)(resources.GetObject("cddColor.Items207"))),
            ((object)(resources.GetObject("cddColor.Items208"))),
            ((object)(resources.GetObject("cddColor.Items209"))),
            ((object)(resources.GetObject("cddColor.Items210"))),
            ((object)(resources.GetObject("cddColor.Items211"))),
            ((object)(resources.GetObject("cddColor.Items212"))),
            ((object)(resources.GetObject("cddColor.Items213"))),
            ((object)(resources.GetObject("cddColor.Items214"))),
            ((object)(resources.GetObject("cddColor.Items215"))),
            ((object)(resources.GetObject("cddColor.Items216"))),
            ((object)(resources.GetObject("cddColor.Items217"))),
            ((object)(resources.GetObject("cddColor.Items218"))),
            ((object)(resources.GetObject("cddColor.Items219"))),
            ((object)(resources.GetObject("cddColor.Items220"))),
            ((object)(resources.GetObject("cddColor.Items221"))),
            ((object)(resources.GetObject("cddColor.Items222"))),
            ((object)(resources.GetObject("cddColor.Items223"))),
            ((object)(resources.GetObject("cddColor.Items224"))),
            ((object)(resources.GetObject("cddColor.Items225"))),
            ((object)(resources.GetObject("cddColor.Items226"))),
            ((object)(resources.GetObject("cddColor.Items227"))),
            ((object)(resources.GetObject("cddColor.Items228"))),
            ((object)(resources.GetObject("cddColor.Items229"))),
            ((object)(resources.GetObject("cddColor.Items230"))),
            ((object)(resources.GetObject("cddColor.Items231"))),
            ((object)(resources.GetObject("cddColor.Items232"))),
            ((object)(resources.GetObject("cddColor.Items233"))),
            ((object)(resources.GetObject("cddColor.Items234"))),
            ((object)(resources.GetObject("cddColor.Items235"))),
            ((object)(resources.GetObject("cddColor.Items236"))),
            ((object)(resources.GetObject("cddColor.Items237"))),
            ((object)(resources.GetObject("cddColor.Items238"))),
            ((object)(resources.GetObject("cddColor.Items239"))),
            ((object)(resources.GetObject("cddColor.Items240"))),
            ((object)(resources.GetObject("cddColor.Items241"))),
            ((object)(resources.GetObject("cddColor.Items242"))),
            ((object)(resources.GetObject("cddColor.Items243"))),
            ((object)(resources.GetObject("cddColor.Items244"))),
            ((object)(resources.GetObject("cddColor.Items245"))),
            ((object)(resources.GetObject("cddColor.Items246"))),
            ((object)(resources.GetObject("cddColor.Items247"))),
            ((object)(resources.GetObject("cddColor.Items248"))),
            ((object)(resources.GetObject("cddColor.Items249"))),
            ((object)(resources.GetObject("cddColor.Items250"))),
            ((object)(resources.GetObject("cddColor.Items251"))),
            ((object)(resources.GetObject("cddColor.Items252"))),
            ((object)(resources.GetObject("cddColor.Items253"))),
            ((object)(resources.GetObject("cddColor.Items254"))),
            ((object)(resources.GetObject("cddColor.Items255"))),
            ((object)(resources.GetObject("cddColor.Items256"))),
            ((object)(resources.GetObject("cddColor.Items257"))),
            ((object)(resources.GetObject("cddColor.Items258"))),
            ((object)(resources.GetObject("cddColor.Items259"))),
            ((object)(resources.GetObject("cddColor.Items260"))),
            ((object)(resources.GetObject("cddColor.Items261"))),
            ((object)(resources.GetObject("cddColor.Items262"))),
            ((object)(resources.GetObject("cddColor.Items263"))),
            ((object)(resources.GetObject("cddColor.Items264"))),
            ((object)(resources.GetObject("cddColor.Items265"))),
            ((object)(resources.GetObject("cddColor.Items266"))),
            ((object)(resources.GetObject("cddColor.Items267"))),
            ((object)(resources.GetObject("cddColor.Items268"))),
            ((object)(resources.GetObject("cddColor.Items269"))),
            ((object)(resources.GetObject("cddColor.Items270"))),
            ((object)(resources.GetObject("cddColor.Items271"))),
            ((object)(resources.GetObject("cddColor.Items272"))),
            ((object)(resources.GetObject("cddColor.Items273"))),
            ((object)(resources.GetObject("cddColor.Items274"))),
            ((object)(resources.GetObject("cddColor.Items275"))),
            ((object)(resources.GetObject("cddColor.Items276"))),
            ((object)(resources.GetObject("cddColor.Items277"))),
            ((object)(resources.GetObject("cddColor.Items278"))),
            ((object)(resources.GetObject("cddColor.Items279"))),
            ((object)(resources.GetObject("cddColor.Items280"))),
            ((object)(resources.GetObject("cddColor.Items281"))),
            ((object)(resources.GetObject("cddColor.Items282"))),
            ((object)(resources.GetObject("cddColor.Items283"))),
            ((object)(resources.GetObject("cddColor.Items284"))),
            ((object)(resources.GetObject("cddColor.Items285"))),
            ((object)(resources.GetObject("cddColor.Items286"))),
            ((object)(resources.GetObject("cddColor.Items287"))),
            ((object)(resources.GetObject("cddColor.Items288"))),
            ((object)(resources.GetObject("cddColor.Items289"))),
            ((object)(resources.GetObject("cddColor.Items290"))),
            ((object)(resources.GetObject("cddColor.Items291"))),
            ((object)(resources.GetObject("cddColor.Items292"))),
            ((object)(resources.GetObject("cddColor.Items293"))),
            ((object)(resources.GetObject("cddColor.Items294"))),
            ((object)(resources.GetObject("cddColor.Items295"))),
            ((object)(resources.GetObject("cddColor.Items296"))),
            ((object)(resources.GetObject("cddColor.Items297"))),
            ((object)(resources.GetObject("cddColor.Items298"))),
            ((object)(resources.GetObject("cddColor.Items299"))),
            ((object)(resources.GetObject("cddColor.Items300"))),
            ((object)(resources.GetObject("cddColor.Items301"))),
            ((object)(resources.GetObject("cddColor.Items302"))),
            ((object)(resources.GetObject("cddColor.Items303"))),
            ((object)(resources.GetObject("cddColor.Items304"))),
            ((object)(resources.GetObject("cddColor.Items305"))),
            ((object)(resources.GetObject("cddColor.Items306"))),
            ((object)(resources.GetObject("cddColor.Items307"))),
            ((object)(resources.GetObject("cddColor.Items308"))),
            ((object)(resources.GetObject("cddColor.Items309"))),
            ((object)(resources.GetObject("cddColor.Items310"))),
            ((object)(resources.GetObject("cddColor.Items311"))),
            ((object)(resources.GetObject("cddColor.Items312"))),
            ((object)(resources.GetObject("cddColor.Items313"))),
            ((object)(resources.GetObject("cddColor.Items314"))),
            ((object)(resources.GetObject("cddColor.Items315"))),
            ((object)(resources.GetObject("cddColor.Items316"))),
            ((object)(resources.GetObject("cddColor.Items317"))),
            ((object)(resources.GetObject("cddColor.Items318"))),
            ((object)(resources.GetObject("cddColor.Items319"))),
            ((object)(resources.GetObject("cddColor.Items320"))),
            ((object)(resources.GetObject("cddColor.Items321"))),
            ((object)(resources.GetObject("cddColor.Items322"))),
            ((object)(resources.GetObject("cddColor.Items323"))),
            ((object)(resources.GetObject("cddColor.Items324"))),
            ((object)(resources.GetObject("cddColor.Items325"))),
            ((object)(resources.GetObject("cddColor.Items326"))),
            ((object)(resources.GetObject("cddColor.Items327"))),
            ((object)(resources.GetObject("cddColor.Items328"))),
            ((object)(resources.GetObject("cddColor.Items329"))),
            ((object)(resources.GetObject("cddColor.Items330"))),
            ((object)(resources.GetObject("cddColor.Items331"))),
            ((object)(resources.GetObject("cddColor.Items332"))),
            ((object)(resources.GetObject("cddColor.Items333"))),
            ((object)(resources.GetObject("cddColor.Items334"))),
            ((object)(resources.GetObject("cddColor.Items335"))),
            ((object)(resources.GetObject("cddColor.Items336"))),
            ((object)(resources.GetObject("cddColor.Items337"))),
            ((object)(resources.GetObject("cddColor.Items338"))),
            ((object)(resources.GetObject("cddColor.Items339"))),
            ((object)(resources.GetObject("cddColor.Items340"))),
            ((object)(resources.GetObject("cddColor.Items341"))),
            ((object)(resources.GetObject("cddColor.Items342"))),
            ((object)(resources.GetObject("cddColor.Items343"))),
            ((object)(resources.GetObject("cddColor.Items344"))),
            ((object)(resources.GetObject("cddColor.Items345"))),
            ((object)(resources.GetObject("cddColor.Items346"))),
            ((object)(resources.GetObject("cddColor.Items347"))),
            ((object)(resources.GetObject("cddColor.Items348"))),
            ((object)(resources.GetObject("cddColor.Items349"))),
            ((object)(resources.GetObject("cddColor.Items350"))),
            ((object)(resources.GetObject("cddColor.Items351"))),
            ((object)(resources.GetObject("cddColor.Items352"))),
            ((object)(resources.GetObject("cddColor.Items353"))),
            ((object)(resources.GetObject("cddColor.Items354"))),
            ((object)(resources.GetObject("cddColor.Items355"))),
            ((object)(resources.GetObject("cddColor.Items356"))),
            ((object)(resources.GetObject("cddColor.Items357"))),
            ((object)(resources.GetObject("cddColor.Items358"))),
            ((object)(resources.GetObject("cddColor.Items359"))),
            ((object)(resources.GetObject("cddColor.Items360"))),
            ((object)(resources.GetObject("cddColor.Items361"))),
            ((object)(resources.GetObject("cddColor.Items362"))),
            ((object)(resources.GetObject("cddColor.Items363"))),
            ((object)(resources.GetObject("cddColor.Items364"))),
            ((object)(resources.GetObject("cddColor.Items365"))),
            ((object)(resources.GetObject("cddColor.Items366"))),
            ((object)(resources.GetObject("cddColor.Items367"))),
            ((object)(resources.GetObject("cddColor.Items368"))),
            ((object)(resources.GetObject("cddColor.Items369"))),
            ((object)(resources.GetObject("cddColor.Items370"))),
            ((object)(resources.GetObject("cddColor.Items371"))),
            ((object)(resources.GetObject("cddColor.Items372"))),
            ((object)(resources.GetObject("cddColor.Items373"))),
            ((object)(resources.GetObject("cddColor.Items374"))),
            ((object)(resources.GetObject("cddColor.Items375"))),
            ((object)(resources.GetObject("cddColor.Items376"))),
            ((object)(resources.GetObject("cddColor.Items377"))),
            ((object)(resources.GetObject("cddColor.Items378"))),
            ((object)(resources.GetObject("cddColor.Items379"))),
            ((object)(resources.GetObject("cddColor.Items380"))),
            ((object)(resources.GetObject("cddColor.Items381"))),
            ((object)(resources.GetObject("cddColor.Items382"))),
            ((object)(resources.GetObject("cddColor.Items383"))),
            ((object)(resources.GetObject("cddColor.Items384"))),
            ((object)(resources.GetObject("cddColor.Items385"))),
            ((object)(resources.GetObject("cddColor.Items386"))),
            ((object)(resources.GetObject("cddColor.Items387"))),
            ((object)(resources.GetObject("cddColor.Items388"))),
            ((object)(resources.GetObject("cddColor.Items389"))),
            ((object)(resources.GetObject("cddColor.Items390"))),
            ((object)(resources.GetObject("cddColor.Items391"))),
            ((object)(resources.GetObject("cddColor.Items392"))),
            ((object)(resources.GetObject("cddColor.Items393"))),
            ((object)(resources.GetObject("cddColor.Items394"))),
            ((object)(resources.GetObject("cddColor.Items395"))),
            ((object)(resources.GetObject("cddColor.Items396"))),
            ((object)(resources.GetObject("cddColor.Items397"))),
            ((object)(resources.GetObject("cddColor.Items398"))),
            ((object)(resources.GetObject("cddColor.Items399"))),
            ((object)(resources.GetObject("cddColor.Items400"))),
            ((object)(resources.GetObject("cddColor.Items401"))),
            ((object)(resources.GetObject("cddColor.Items402"))),
            ((object)(resources.GetObject("cddColor.Items403"))),
            ((object)(resources.GetObject("cddColor.Items404"))),
            ((object)(resources.GetObject("cddColor.Items405"))),
            ((object)(resources.GetObject("cddColor.Items406"))),
            ((object)(resources.GetObject("cddColor.Items407"))),
            ((object)(resources.GetObject("cddColor.Items408"))),
            ((object)(resources.GetObject("cddColor.Items409"))),
            ((object)(resources.GetObject("cddColor.Items410"))),
            ((object)(resources.GetObject("cddColor.Items411"))),
            ((object)(resources.GetObject("cddColor.Items412"))),
            ((object)(resources.GetObject("cddColor.Items413"))),
            ((object)(resources.GetObject("cddColor.Items414"))),
            ((object)(resources.GetObject("cddColor.Items415"))),
            ((object)(resources.GetObject("cddColor.Items416"))),
            ((object)(resources.GetObject("cddColor.Items417"))),
            ((object)(resources.GetObject("cddColor.Items418"))),
            ((object)(resources.GetObject("cddColor.Items419"))),
            ((object)(resources.GetObject("cddColor.Items420"))),
            ((object)(resources.GetObject("cddColor.Items421"))),
            ((object)(resources.GetObject("cddColor.Items422"))),
            ((object)(resources.GetObject("cddColor.Items423"))),
            ((object)(resources.GetObject("cddColor.Items424"))),
            ((object)(resources.GetObject("cddColor.Items425"))),
            ((object)(resources.GetObject("cddColor.Items426"))),
            ((object)(resources.GetObject("cddColor.Items427"))),
            ((object)(resources.GetObject("cddColor.Items428"))),
            ((object)(resources.GetObject("cddColor.Items429"))),
            ((object)(resources.GetObject("cddColor.Items430"))),
            ((object)(resources.GetObject("cddColor.Items431"))),
            ((object)(resources.GetObject("cddColor.Items432"))),
            ((object)(resources.GetObject("cddColor.Items433"))),
            ((object)(resources.GetObject("cddColor.Items434"))),
            ((object)(resources.GetObject("cddColor.Items435"))),
            ((object)(resources.GetObject("cddColor.Items436"))),
            ((object)(resources.GetObject("cddColor.Items437"))),
            ((object)(resources.GetObject("cddColor.Items438"))),
            ((object)(resources.GetObject("cddColor.Items439"))),
            ((object)(resources.GetObject("cddColor.Items440"))),
            ((object)(resources.GetObject("cddColor.Items441"))),
            ((object)(resources.GetObject("cddColor.Items442"))),
            ((object)(resources.GetObject("cddColor.Items443"))),
            ((object)(resources.GetObject("cddColor.Items444"))),
            ((object)(resources.GetObject("cddColor.Items445"))),
            ((object)(resources.GetObject("cddColor.Items446"))),
            ((object)(resources.GetObject("cddColor.Items447"))),
            ((object)(resources.GetObject("cddColor.Items448"))),
            ((object)(resources.GetObject("cddColor.Items449"))),
            ((object)(resources.GetObject("cddColor.Items450"))),
            ((object)(resources.GetObject("cddColor.Items451"))),
            ((object)(resources.GetObject("cddColor.Items452"))),
            ((object)(resources.GetObject("cddColor.Items453"))),
            ((object)(resources.GetObject("cddColor.Items454"))),
            ((object)(resources.GetObject("cddColor.Items455"))),
            ((object)(resources.GetObject("cddColor.Items456"))),
            ((object)(resources.GetObject("cddColor.Items457"))),
            ((object)(resources.GetObject("cddColor.Items458"))),
            ((object)(resources.GetObject("cddColor.Items459"))),
            ((object)(resources.GetObject("cddColor.Items460"))),
            ((object)(resources.GetObject("cddColor.Items461"))),
            ((object)(resources.GetObject("cddColor.Items462"))),
            ((object)(resources.GetObject("cddColor.Items463"))),
            ((object)(resources.GetObject("cddColor.Items464"))),
            ((object)(resources.GetObject("cddColor.Items465"))),
            ((object)(resources.GetObject("cddColor.Items466"))),
            ((object)(resources.GetObject("cddColor.Items467"))),
            ((object)(resources.GetObject("cddColor.Items468"))),
            ((object)(resources.GetObject("cddColor.Items469"))),
            ((object)(resources.GetObject("cddColor.Items470"))),
            ((object)(resources.GetObject("cddColor.Items471"))),
            ((object)(resources.GetObject("cddColor.Items472"))),
            ((object)(resources.GetObject("cddColor.Items473"))),
            ((object)(resources.GetObject("cddColor.Items474"))),
            ((object)(resources.GetObject("cddColor.Items475"))),
            ((object)(resources.GetObject("cddColor.Items476"))),
            ((object)(resources.GetObject("cddColor.Items477"))),
            ((object)(resources.GetObject("cddColor.Items478"))),
            ((object)(resources.GetObject("cddColor.Items479"))),
            ((object)(resources.GetObject("cddColor.Items480"))),
            ((object)(resources.GetObject("cddColor.Items481"))),
            ((object)(resources.GetObject("cddColor.Items482"))),
            ((object)(resources.GetObject("cddColor.Items483"))),
            ((object)(resources.GetObject("cddColor.Items484"))),
            ((object)(resources.GetObject("cddColor.Items485"))),
            ((object)(resources.GetObject("cddColor.Items486"))),
            ((object)(resources.GetObject("cddColor.Items487"))),
            ((object)(resources.GetObject("cddColor.Items488"))),
            ((object)(resources.GetObject("cddColor.Items489"))),
            ((object)(resources.GetObject("cddColor.Items490"))),
            ((object)(resources.GetObject("cddColor.Items491"))),
            ((object)(resources.GetObject("cddColor.Items492"))),
            ((object)(resources.GetObject("cddColor.Items493"))),
            ((object)(resources.GetObject("cddColor.Items494"))),
            ((object)(resources.GetObject("cddColor.Items495"))),
            ((object)(resources.GetObject("cddColor.Items496"))),
            ((object)(resources.GetObject("cddColor.Items497"))),
            ((object)(resources.GetObject("cddColor.Items498"))),
            ((object)(resources.GetObject("cddColor.Items499"))),
            ((object)(resources.GetObject("cddColor.Items500"))),
            ((object)(resources.GetObject("cddColor.Items501"))),
            ((object)(resources.GetObject("cddColor.Items502"))),
            ((object)(resources.GetObject("cddColor.Items503"))),
            ((object)(resources.GetObject("cddColor.Items504"))),
            ((object)(resources.GetObject("cddColor.Items505"))),
            ((object)(resources.GetObject("cddColor.Items506"))),
            ((object)(resources.GetObject("cddColor.Items507"))),
            ((object)(resources.GetObject("cddColor.Items508"))),
            ((object)(resources.GetObject("cddColor.Items509"))),
            ((object)(resources.GetObject("cddColor.Items510"))),
            ((object)(resources.GetObject("cddColor.Items511"))),
            ((object)(resources.GetObject("cddColor.Items512"))),
            ((object)(resources.GetObject("cddColor.Items513"))),
            ((object)(resources.GetObject("cddColor.Items514"))),
            ((object)(resources.GetObject("cddColor.Items515"))),
            ((object)(resources.GetObject("cddColor.Items516"))),
            ((object)(resources.GetObject("cddColor.Items517"))),
            ((object)(resources.GetObject("cddColor.Items518"))),
            ((object)(resources.GetObject("cddColor.Items519"))),
            ((object)(resources.GetObject("cddColor.Items520"))),
            ((object)(resources.GetObject("cddColor.Items521"))),
            ((object)(resources.GetObject("cddColor.Items522")))});
            this.cddColor.Name = "cddColor";
            this.cddColor.Value = System.Drawing.Color.Empty;
            // 
            // cmdShowDialog
            // 
            this.cmdShowDialog.AccessibleDescription = null;
            this.cmdShowDialog.AccessibleName = null;
            resources.ApplyResources(this.cmdShowDialog, "cmdShowDialog");
            this.cmdShowDialog.BackgroundImage = null;
            this.cmdShowDialog.Font = null;
            this.cmdShowDialog.Name = "cmdShowDialog";
            this.cmdShowDialog.UseVisualStyleBackColor = true;
            this.cmdShowDialog.Click += new System.EventHandler(this.cmdShowDialog_Click);
            // 
            // ColorBox
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.cmdShowDialog);
            this.Controls.Add(this.cddColor);
            this.Controls.Add(this.lblColor);
            this.Font = null;
            this.Name = "ColorBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected color
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the selected color")]
        public Color Value
        {
            get { return cddColor.Value; }
            set { cddColor.Value = value; }
          
        }

        /// <summary>
        /// Gets or sets the text for the label portion
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the text for the label portion")]
        public string LabelText
        {
            get { return lblColor.Text; }
            set 
            { 
                lblColor.Text = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or set the font for the label portion of the component.
        /// </summary>
        [Category("Appearance"), Description("Gets or set the font for the label portion of the component.")]
        public new Font Font
        {
            get { return lblColor.Font; }
            set 
            { 
                lblColor.Font = value;
                Reset();
            }
        }



        #endregion

        #region Protected Methods

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #endregion

       

        private void cmdShowDialog_Click(object sender, EventArgs e)
        {
            ColorDialog cdlg = new ColorDialog();
            if (cdlg.ShowDialog(ParentForm) != DialogResult.OK) return;
            foreach (object item in cddColor.Items)
            {
                if (item is KnownColor)
                {
                    KnownColor kn = (KnownColor)item;
                    if (Color.FromKnownColor(kn) == cdlg.Color)
                    {
                        cddColor.SelectedItem = kn;
                        return;
                    }
                }
            }
            if (cddColor.Items.Contains(cdlg.Color))
            {
                cddColor.SelectedItem = cdlg.Color;
                return;
            }
            else
            {
                cddColor.Items.Add(cdlg.Color);
                cddColor.SelectedIndex = cddColor.Items.Count - 1;
            }
        }

        /// <summary>
        /// Changes the starting location of the color drop down based on the current text.
        /// </summary>
        private void Reset()
        {
            cddColor.Left = lblColor.Width + 5;
            cddColor.Width = cmdShowDialog.Left - cddColor.Left - 10;
        }

    }
}
