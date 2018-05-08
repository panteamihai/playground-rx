namespace WithUI
{
    partial class UI
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
            this.btnDo = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(12, 12);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(236, 70);
            this.btnDo.TabIndex = 0;
            this.btnDo.Text = "Do";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.BtnDoClick);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(254, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(413, 266);
            this.txtOutput.TabIndex = 1;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 286);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnDo);
            this.Name = "UI";
            this.Text = "For examples that require a SynchronizationContext";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.TextBox txtOutput;
    }
}

