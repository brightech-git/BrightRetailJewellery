namespace Test
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.exportBtn = new System.Windows.Forms.ToolStripButton();
            this.grid = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsAlive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.useAutomaticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useOpenOfficeorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTest = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataClassBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(759, 418);
            this.dataGridView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportBtn,
            this.createTest,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(848, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // exportBtn
            // 
            this.exportBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exportBtn.Image = ((System.Drawing.Image)(resources.GetObject("exportBtn.Image")));
            this.exportBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(43, 22);
            this.exportBtn.Text = "Export";
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // grid
            // 
            this.grid.AutoGenerateColumns = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.IsAlive,
            this.firstNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.positionDataGridViewTextBoxColumn});
            this.grid.DataSource = this.dataClassBindingSource;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 25);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(848, 422);
            this.grid.TabIndex = 1;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // IsAlive
            // 
            this.IsAlive.DataPropertyName = "IsAlive";
            this.IsAlive.HeaderText = "IsAlive";
            this.IsAlive.Name = "IsAlive";
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "LastName";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // positionDataGridViewTextBoxColumn
            // 
            this.positionDataGridViewTextBoxColumn.DataPropertyName = "Position";
            this.positionDataGridViewTextBoxColumn.HeaderText = "Position";
            this.positionDataGridViewTextBoxColumn.Name = "positionDataGridViewTextBoxColumn";
            // 
            // dataClassBindingSource
            // 
            this.dataClassBindingSource.DataSource = typeof(Test.DataClass);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useAutomaticToolStripMenuItem,
            this.useExcelToolStripMenuItem,
            this.useOpenOfficeorgToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(93, 22);
            this.toolStripDropDownButton1.Text = "TableProcessor";
            // 
            // useAutomaticToolStripMenuItem
            // 
            this.useAutomaticToolStripMenuItem.Checked = true;
            this.useAutomaticToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAutomaticToolStripMenuItem.Name = "useAutomaticToolStripMenuItem";
            this.useAutomaticToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.useAutomaticToolStripMenuItem.Text = "Use automatic";
            this.useAutomaticToolStripMenuItem.Click += new System.EventHandler(this.useAutomaticToolStripMenuItem_Click);
            // 
            // useExcelToolStripMenuItem
            // 
            this.useExcelToolStripMenuItem.Name = "useExcelToolStripMenuItem";
            this.useExcelToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.useExcelToolStripMenuItem.Text = "Use Excel";
            this.useExcelToolStripMenuItem.Click += new System.EventHandler(this.useExcelToolStripMenuItem_Click);
            // 
            // useOpenOfficeorgToolStripMenuItem
            // 
            this.useOpenOfficeorgToolStripMenuItem.Name = "useOpenOfficeorgToolStripMenuItem";
            this.useOpenOfficeorgToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.useOpenOfficeorgToolStripMenuItem.Text = "Use OpenOffice.org";
            this.useOpenOfficeorgToolStripMenuItem.Click += new System.EventHandler(this.useOpenOfficeorgToolStripMenuItem_Click);
            // 
            // createTest
            // 
            this.createTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createTest.Image = ((System.Drawing.Image)(resources.GetObject("createTest.Image")));
            this.createTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createTest.Name = "createTest";
            this.createTest.Size = new System.Drawing.Size(159, 22);
            this.createTest.Text = "Create some colorized test doc";
            this.createTest.Click += new System.EventHandler(this.createTest_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(848, 447);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Sample form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataClassBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton exportBtn;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.BindingSource dataClassBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsAlive;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem useAutomaticToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useOpenOfficeorgToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton createTest;
    }
}

