using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Windows.Forms;

namespace VBPowerShell
{
    public partial class PowerShell : Form
    {
        public PowerShell()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtOutput.Text = RunScript(txtCommand.Text);
        }

        private string RunScript(string command)
        {
            string output = string.Empty;
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            try
            {
                Command myCommand = new Command(command);
                //TODO: Implement verification if exist params
                //CommandParameter params = new CommandParameter("key", "value");
                //myCommand.Parameters.Add(params);

                pipeline.Commands.Add(myCommand);

                Collection<PSObject> psObjects;
                psObjects = pipeline.Invoke();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject item in psObjects)
                {
                    stringBuilder.AppendLine(item.ToString());
                }
                output = stringBuilder.ToString();
            }catch(Exception e)
            {
                output = e.Message;
            }            

            return output;
        }
    }
}
