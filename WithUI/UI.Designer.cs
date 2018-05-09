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
            this.btnContextDisposable = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnFromEventPattern = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnContextDisposable
            // 
            this.btnContextDisposable.Location = new System.Drawing.Point(12, 12);
            this.btnContextDisposable.Name = "btnContextDisposable";
            this.btnContextDisposable.Size = new System.Drawing.Size(236, 42);
            this.btnContextDisposable.TabIndex = 0;
            this.btnContextDisposable.Text = "Run ContextDisposable example";
            this.btnContextDisposable.UseVisualStyleBackColor = true;
            this.btnContextDisposable.Click += new System.EventHandler(this.BtnContextDisposableClick);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(254, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(413, 266);
            this.txtOutput.TabIndex = 1;
            // 
            // btnFromEventPattern
            // 
            this.btnFromEventPattern.Location = new System.Drawing.Point(12, 60);
            this.btnFromEventPattern.Name = "btnFromEventPattern";
            this.btnFromEventPattern.Size = new System.Drawing.Size(236, 42);
            this.btnFromEventPattern.TabIndex = 2;
            this.btnFromEventPattern.Text = "Run FromEventPattern example";
            this.btnFromEventPattern.UseVisualStyleBackColor = true;
            this.btnFromEventPattern.Click += new System.EventHandler(this.BtnFromEventPatternClick);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 286);
            this.Controls.Add(this.btnFromEventPattern);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnContextDisposable);
            this.Name = "UI";
            this.Text = "For examples that require a SynchronizationContext";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnContextDisposable;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnFromEventPattern;
    }
}

