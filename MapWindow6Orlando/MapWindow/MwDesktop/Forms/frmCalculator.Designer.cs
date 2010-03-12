namespace MapWindow.Forms
{
    partial class frmCalculator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFieldTitle = new System.Windows.Forms.Label();
            this.lstViewFields = new System.Windows.Forms.ListView();
            this.lstBoxFunctions = new System.Windows.Forms.ListBox();
            this.lblFunctions = new System.Windows.Forms.Label();
            this.btnClaculate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnDivide = new System.Windows.Forms.Button();
            this.btnMultiply = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.lblDestinationFieldTitle = new System.Windows.Forms.Label();
            this.comDestFieldComboBox = new System.Windows.Forms.ComboBox();
            this.lblAssignment = new System.Windows.Forms.Label();
            this.rtxtComputaion = new System.Windows.Forms.RichTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblComputaion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFieldTitle
            // 
            this.lblFieldTitle.AutoSize = true;
            this.lblFieldTitle.Location = new System.Drawing.Point(15, 12);
            this.lblFieldTitle.Name = "lblFieldTitle";
            this.lblFieldTitle.Size = new System.Drawing.Size(129, 13);
            this.lblFieldTitle.TabIndex = 0;
            this.lblFieldTitle.Text = "Table Fields: Double click";
            // 
            // lstViewFields
            // 
            this.lstViewFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstViewFields.FullRowSelect = true;
            this.lstViewFields.Location = new System.Drawing.Point(18, 35);
            this.lstViewFields.Name = "lstViewFields";
            this.lstViewFields.ShowItemToolTips = true;
            this.lstViewFields.Size = new System.Drawing.Size(247, 149);
            this.lstViewFields.TabIndex = 1;
            this.lstViewFields.TileSize = new System.Drawing.Size(247, 30);
            this.lstViewFields.UseCompatibleStateImageBehavior = false;
            this.lstViewFields.View = System.Windows.Forms.View.SmallIcon;
            this.lstViewFields.SelectedIndexChanged += new System.EventHandler(this.lstViewFields_SelectedIndexChanged);
            this.lstViewFields.DoubleClick += new System.EventHandler(this.lstViewFields_DoubleClick);
            // 
            // lstBoxFunctions
            // 
            this.lstBoxFunctions.FormattingEnabled = true;
            this.lstBoxFunctions.Items.AddRange(new object[] {
            "+",
            "-",
            "*",
            "/",
            "%",
            "\\",
            "^",
            "!",
            "abs(x)",
            "atan(x)",
            "cos(x)",
            "sin(x)",
            "exp(x)",
            "fix(x)",
            "int(x)",
            "dec(x)",
            "ln(x)",
            "log(x)",
            "logN(x,n)",
            "rnd(x)",
            "sgn(x)",
            "sqr(x)",
            "cbr(x)",
            "tan(x)",
            "acos(x)",
            "asin(x)",
            "cosh(x)",
            "sinh(x)",
            "tanh(x)",
            "acosh(x)",
            "asinh(x)",
            "atanh(x)",
            "root(x,n)",
            "mod(a,b)",
            "fact(x)",
            "comb(n,k)",
            "perm(n,k)",
            "min(a,b)",
            "max(a,b)",
            "mcd(a,b)",
            "mcm(a,b)",
            "gcd(a,b)",
            "lcm(a,b)",
            "csc(x)",
            "sec(x)",
            "cot(x)",
            "acsc(x)",
            "asec(x)",
            "acot(x)",
            "csch(x)",
            "sech(x)",
            "coth(x)",
            "acsch(x)",
            "asech(x)",
            "acoth(x)",
            "rad(x)",
            "deg(x)",
            "grad(x)",
            "round(x,d)",
            "> ",
            ">=",
            "< ",
            "<=",
            "=",
            "<> ",
            "and",
            "or",
            "not",
            "xor",
            "nand",
            "nor",
            "nxor",
            "Psi(x) ",
            "DNorm(x,μ,σ)",
            "CNorm(x,m,d)",
            "DPoisson(x,k)",
            "CPoisson(x,k)",
            "DBinom(k,n,x)",
            "CBinom(k,n,x)",
            "Si(x)",
            "Ci(x)",
            "FresnelS(x)",
            "FresnelC(x)",
            "J0(x)",
            "Y0(x)",
            "I0(x)",
            "K0(x)",
            "BesselJ(x,n)",
            "BesselY(x,n)",
            "BesselI(x,n)",
            "BesselK(x,n)",
            "HypGeom(x,a,b,c)",
            "PolyCh(x,n)",
            "PolyLe(x,n)",
            "PolyLa(x,n)",
            "PolyHe(x,n)",
            "AiryA(x)   ",
            "AiryB(x)  ",
            "Elli1(x)",
            "Elli2(x)  ",
            "Erf(x)",
            "gamma(x)",
            "gammaln(x)",
            "gammai(a,x)",
            "digamma(x) psi(x)",
            "beta(a,b)",
            "betaI(x,a,b)",
            "Ei(x)",
            "Ein(x,n)",
            "zeta(x)",
            "Clip(x,a,b)   ",
            "WTRI(t,p)",
            "WSQR(t,p)",
            "WRECT(t,p,d)",
            "WTRAPEZ(t,p,d)",
            "WSAW(t,p)",
            "WRAISE(t,p)",
            "WLIN(t,p,d)",
            "WPULSE(t,p,d)",
            "WSTEPS(t,p,n)",
            "WEXP(t,p,a)",
            "WEXPB(t,p,a)",
            "WPULSEF(t,p,a)",
            "WRING(t,p,a,fm)",
            "WPARAB(t,p)",
            "WRIPPLE(t,p,a)",
            "WAM(t,fo,fm,m)",
            "WFM(t,fo,fm,m)",
            "Year(d)",
            "Month(d)",
            "Day(d)",
            "Hour(d)",
            "Minute(d)",
            "Second(d)",
            "DateSerial(a,m,d)",
            "TimeSerial(h,m,s)",
            "time#",
            "date#",
            "now#",
            "Sum(a,b)",
            "Mean(a,b)",
            "Meanq(a,b)",
            "Meang(a,b)",
            "Var(a,b)",
            "Varp(a,b)",
            "Stdev(a,b)",
            "Stdevp(a,b)",
            "Step(x,a)"});
            this.lstBoxFunctions.Location = new System.Drawing.Point(305, 35);
            this.lstBoxFunctions.Name = "lstBoxFunctions";
            this.lstBoxFunctions.Size = new System.Drawing.Size(130, 147);
            this.lstBoxFunctions.TabIndex = 2;
            this.lstBoxFunctions.SelectedIndexChanged += new System.EventHandler(this.lstBoxFunctions_SelectedIndexChanged);
            this.lstBoxFunctions.DoubleClick += new System.EventHandler(this.lstBoxFunctions_DoubleClick);
            // 
            // lblFunctions
            // 
            this.lblFunctions.AutoSize = true;
            this.lblFunctions.Location = new System.Drawing.Point(302, 12);
            this.lblFunctions.Name = "lblFunctions";
            this.lblFunctions.Size = new System.Drawing.Size(118, 13);
            this.lblFunctions.TabIndex = 3;
            this.lblFunctions.Text = "Functions: Double click";
            // 
            // btnClaculate
            // 
            this.btnClaculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClaculate.Location = new System.Drawing.Point(369, 332);
            this.btnClaculate.Name = "btnClaculate";
            this.btnClaculate.Size = new System.Drawing.Size(77, 24);
            this.btnClaculate.TabIndex = 5;
            this.btnClaculate.Text = "Calculate";
            this.btnClaculate.UseVisualStyleBackColor = true;
            this.btnClaculate.Click += new System.EventHandler(this.btnClaculate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(369, 379);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 24);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Location = new System.Drawing.Point(305, 188);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(26, 21);
            this.btnPlus.TabIndex = 7;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            // 
            // btnDivide
            // 
            this.btnDivide.Location = new System.Drawing.Point(401, 188);
            this.btnDivide.Name = "btnDivide";
            this.btnDivide.Size = new System.Drawing.Size(26, 21);
            this.btnDivide.TabIndex = 9;
            this.btnDivide.Text = "/";
            this.btnDivide.UseVisualStyleBackColor = true;
            // 
            // btnMultiply
            // 
            this.btnMultiply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMultiply.Location = new System.Drawing.Point(369, 188);
            this.btnMultiply.Name = "btnMultiply";
            this.btnMultiply.Size = new System.Drawing.Size(26, 21);
            this.btnMultiply.TabIndex = 10;
            this.btnMultiply.Text = " * ";
            this.btnMultiply.UseVisualStyleBackColor = true;
            // 
            // btnMinus
            // 
            this.btnMinus.Location = new System.Drawing.Point(337, 188);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(26, 21);
            this.btnMinus.TabIndex = 11;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            // 
            // lblDestinationFieldTitle
            // 
            this.lblDestinationFieldTitle.AutoSize = true;
            this.lblDestinationFieldTitle.Location = new System.Drawing.Point(15, 212);
            this.lblDestinationFieldTitle.Name = "lblDestinationFieldTitle";
            this.lblDestinationFieldTitle.Size = new System.Drawing.Size(105, 13);
            this.lblDestinationFieldTitle.TabIndex = 12;
            this.lblDestinationFieldTitle.Text = "DestinationFieldTitle:";
            // 
            // comDestFieldComboBox
            // 
            this.comDestFieldComboBox.FormattingEnabled = true;
            this.comDestFieldComboBox.Location = new System.Drawing.Point(18, 228);
            this.comDestFieldComboBox.Name = "comDestFieldComboBox";
            this.comDestFieldComboBox.Size = new System.Drawing.Size(244, 21);
            this.comDestFieldComboBox.TabIndex = 13;
            this.comDestFieldComboBox.Text = "Select One";
            // 
            // lblAssignment
            // 
            this.lblAssignment.AutoSize = true;
            this.lblAssignment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssignment.Location = new System.Drawing.Point(268, 233);
            this.lblAssignment.Name = "lblAssignment";
            this.lblAssignment.Size = new System.Drawing.Size(16, 16);
            this.lblAssignment.TabIndex = 14;
            this.lblAssignment.Text = "=";
            // 
            // rtxtComputaion
            // 
            this.rtxtComputaion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtComputaion.ForeColor = System.Drawing.SystemColors.Highlight;
            this.rtxtComputaion.Location = new System.Drawing.Point(18, 278);
            this.rtxtComputaion.Name = "rtxtComputaion";
            this.rtxtComputaion.Size = new System.Drawing.Size(345, 114);
            this.rtxtComputaion.TabIndex = 15;
            this.rtxtComputaion.Text = "";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(426, 12);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(32, 13);
            this.linkLabel1.TabIndex = 16;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Help!";
            // 
            // lblComputaion
            // 
            this.lblComputaion.AutoSize = true;
            this.lblComputaion.Location = new System.Drawing.Point(18, 262);
            this.lblComputaion.Name = "lblComputaion";
            this.lblComputaion.Size = new System.Drawing.Size(113, 13);
            this.lblComputaion.TabIndex = 17;
            this.lblComputaion.Text = "Computation equation:";
            // 
            // frmCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 415);
            this.Controls.Add(this.lblComputaion);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.rtxtComputaion);
            this.Controls.Add(this.lblAssignment);
            this.Controls.Add(this.comDestFieldComboBox);
            this.Controls.Add(this.lblDestinationFieldTitle);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnMultiply);
            this.Controls.Add(this.btnDivide);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClaculate);
            this.Controls.Add(this.lblFunctions);
            this.Controls.Add(this.lstBoxFunctions);
            this.Controls.Add(this.lstViewFields);
            this.Controls.Add(this.lblFieldTitle);
            this.Name = "frmCalculator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Field Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void lstBoxFunctions_DoubleClick(object sender, System.EventArgs e)
        {
            string Fun1Full = "Abs(x) Atn(x) Cos(x) Exp(x) Fix(x) Int(x) Ln(x) Log(x) Rnd(x) Sgn(x) Sin(x) Sqr(x) Cbr(x) Tan(x) Acos(x) Asin(x) " + " Cosh(x) Sinh(x) Tanh(x) Acosh(x) Asinh(x) Atanh(x) Fact(x) Not(x) Erf(x) Gamma(x) Gammaln(x) Digamma(x) Zeta(x) Ei(x) " + " csc(x) sec(x) cot(x) acsc(x) asec(x) acot(x) csch(x) sech(x) coth(x) acsch(x) asech(x) acoth(x) Dec(x) Rad(x) Deg(x) Grad(x) ";
            string Fun2Full = "Comb(n,k) Max(a,b) Min(a,b) Mcm(a,b) Mcd(a,b) Lcm(a,b) Gcd(a,b) Mod(a,b) Beta(a,b) Root(x,n) Round(x,d)";
            string temp;

            temp = lstBoxFunctions.SelectedItem.ToString();
            if (Fun1Full.ToLower().Contains(temp.ToLower()))// check mono variable
            {
                if (temp.Length > 2)
                {
                    temp = temp.Remove(temp.Length - 2, 2);
                    temp = "[" + temp;// +"]";
                }
            }
            else if (Fun2Full.ToLower().Contains(temp.ToLower())) // Check di Variable
            {

                if (temp.Length > 4)
                {
                    temp = temp.Remove(temp.Length - 4, 4);
                    temp = "[" + temp;// +"]";
                }
            }
            else
                temp = "[" + temp + "]"; //symbols
            //temp = temp.Remove(temp.Length - 1, 1);
            if (temp != null)
            {
                Expression = rtxtComputaion.Text;
                Expression = Expression + temp;
                DisplyExpression();
            }
        }

        void lstViewFields_DoubleClick(object sender, System.EventArgs e)
        {
            string temp;
            System.Windows.Forms.ListViewItem item;
            item = lstViewFields.FocusedItem;
            temp = item.Text;
            temp = temp.Remove(temp.Length - 1, 1);
            temp = "[" + temp + "]";
            if (item != null)
            {
                Expression = rtxtComputaion.Text;
                Expression = Expression + temp;
                DisplyExpression();
            }
        }

        #endregion

        private System.Windows.Forms.Label lblFieldTitle;
        private System.Windows.Forms.ListBox lstBoxFunctions;
        private System.Windows.Forms.Label lblFunctions;
        private System.Windows.Forms.Button btnClaculate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnDivide;
        private System.Windows.Forms.Button btnMultiply;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Label lblDestinationFieldTitle;
        private System.Windows.Forms.ComboBox comDestFieldComboBox;
        private System.Windows.Forms.Label lblAssignment;
        private System.Windows.Forms.RichTextBox rtxtComputaion;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblComputaion;
        private System.Windows.Forms.ListView lstViewFields;
    }
}