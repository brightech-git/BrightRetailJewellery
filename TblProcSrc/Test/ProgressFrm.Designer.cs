namespace Test
{
    partial class ProgressFrm
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
            this._label = new System.Windows.Forms.Label();
            this._progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // _label
            // 
            this._label.Location = new System.Drawing.Point(7, 9);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(480, 37);
            this._label.TabIndex = 3;
            // 
            // _progress
            // 
            this._progress.Location = new System.Drawing.Point(7, 49);
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(480, 24);
            this._progress.Step = 1;
            this._progress.TabIndex = 2;
            // 
            // ProgressFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 80);
            this.ControlBox = false;
            this.Controls.Add(this._label);
            this.Controls.Add(this._progress);
            this.Name = "ProgressFrm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _label;
        private System.Windows.Forms.ProgressBar _progress;
    }
}