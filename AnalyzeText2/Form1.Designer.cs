namespace AnalyzeText2
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
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnTestSentence = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutPut = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(65, 58);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(99, 23);
            this.btnTrain.TabIndex = 0;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnTestSentence
            // 
            this.btnTestSentence.Location = new System.Drawing.Point(65, 113);
            this.btnTestSentence.Name = "btnTestSentence";
            this.btnTestSentence.Size = new System.Drawing.Size(99, 23);
            this.btnTestSentence.TabIndex = 1;
            this.btnTestSentence.Text = "Test sentence";
            this.btnTestSentence.UseVisualStyleBackColor = true;
            this.btnTestSentence.Click += new System.EventHandler(this.btnTestSentence_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(65, 87);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(508, 20);
            this.txtInput.TabIndex = 2;
            // 
            // txtOutPut
            // 
            this.txtOutPut.Location = new System.Drawing.Point(65, 152);
            this.txtOutPut.Name = "txtOutPut";
            this.txtOutPut.Size = new System.Drawing.Size(508, 20);
            this.txtOutPut.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(65, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOutPut);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnTestSentence);
            this.Controls.Add(this.btnTrain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnTestSentence;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutPut;
        private System.Windows.Forms.Button button1;
    }
}

