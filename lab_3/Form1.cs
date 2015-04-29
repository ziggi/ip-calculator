using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ip_calculator
{
    public partial class Form1 : Form
    {
        public int[] ip_octet;
        public int[] mask_octet;
        public int ip_class;

        public Form1() {
            InitializeComponent();
            this.ip_octet = new int[4];
            this.mask_octet = new int[4];
        }

        private void Form1_Load(object sender, EventArgs e) {
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label10.Text = "";
            label11.Text = "";

            numericUpDown1.Value = this.ip_octet[0];
            numericUpDown2.Value = this.ip_octet[1];
            numericUpDown3.Value = this.ip_octet[2];
            numericUpDown4.Value = this.ip_octet[3];

            numericUpDown5.Value = this.mask_octet[0];
            numericUpDown6.Value = this.mask_octet[1];
            numericUpDown7.Value = this.mask_octet[2];
            numericUpDown8.Value = this.mask_octet[3];
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            this.ip_octet[0] = Decimal.ToInt32(numericUpDown1.Value);
            updateClass();
            updateSubnets();
            updateNetworkID();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.ip_octet[1] = Decimal.ToInt32(numericUpDown2.Value);
            updateClass();
            updateSubnets();
            updateNetworkID();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e) {
            this.ip_octet[2] = Decimal.ToInt32(numericUpDown3.Value);
            updateSubnets();
            updateNetworkID();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e) {
            this.ip_octet[3] = Decimal.ToInt32(numericUpDown4.Value);
            updateSubnets();
            updateNetworkID();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e) {
            this.mask_octet[0] = Decimal.ToInt32(numericUpDown5.Value);
            updateHosts();
            updateNetworkID();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e) {
            this.mask_octet[1] = Decimal.ToInt32(numericUpDown6.Value);
            updateHosts();
            updateNetworkID();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e) {
            this.mask_octet[2] = Decimal.ToInt32(numericUpDown7.Value);
            updateHosts();
            updateNetworkID();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e) {
            this.mask_octet[3] = Decimal.ToInt32(numericUpDown8.Value);
            updateHosts();
            updateNetworkID();
        }

        public void updateClass() {
            this.ip_class = IpCalculator.CalcClass(this.ip_octet);
            label5.Text = IpCalculator.GetClass(ref this.ip_class);
        }

        public void updateNetworkID() {
            int netInc = IpCalculator.GetIncrement(this.mask_octet, this.ip_octet);

            int[] netid = IpCalculator.GetNetID(this.ip_octet, this.mask_octet);
            int[] broadcastid = IpCalculator.GetNetIDRange(netid, netInc, this.mask_octet);

            int[] netid_start = new int[netid.Length];
            netid.CopyTo(netid_start, 0);
            netid_start[3] += 1;

            int[] netid_range = new int[broadcastid.Length];
            broadcastid.CopyTo(netid_range, 0);
            netid_range[3] -= 1;

            label8.Text = String.Join(".", netid_start) + " - " + String.Join(".", netid_range);
            label10.Text = String.Join(".", broadcastid);
            label11.Text = String.Join(".", netid);
        }

        public void updateHosts() {
            Int64 hosts_count = IpCalculator.GetHostsPerSubnet(mask_octet);
            if (hosts_count == -1) {
                label6.Text = "Error mask input";
            } else {
                label6.Text = Convert.ToString(hosts_count);
            }
        }

        public void updateSubnets() {
            int sub_count = IpCalculator.GetSubnets(this.mask_octet, this.ip_class);
            label7.Text = Convert.ToString(sub_count);
        }
    }
}
