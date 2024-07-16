using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using static iTextSharp.text.TabStop;

namespace MedKarta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            string familia = textBox1.Text.ToString();       
            string imya = textBox2.Text.ToString();
            string otchestvo = textBox3.Text.ToString();

            string dataR = dateTimePicker1.Text.ToString();

            string vin = textBox4.Text.ToString();
            string reg = textBox5.Text.ToString();

            string TypeTC = "";
            if(radio1.Checked)
                TypeTC = radio1.Text.ToString();
            if (radio2.Checked)
                TypeTC = radio2.Text.ToString();
            if (radio3.Checked)
                TypeTC = radio3.Text.ToString();
            if (radio4.Checked)
                TypeTC = radio4.Text.ToString();
            if (radio5.Checked)
                TypeTC = radio4.Text.ToString();

            System.Drawing.Image photo = pictureBoxPhoto.Image;


            if (familia.Count() >= 50) 
                MessageBox.Show("Вы ввели слишком длинную фамилию");
            if (imya.Count() >= 50) 
                MessageBox.Show("Вы ввели слишком длинное имя");
            if (otchestvo.Count() >= 50) 
                MessageBox.Show("Вы ввели слишком длинное отчество");
            if (vin.Count() > 17 || vin.Count() < 17)
                MessageBox.Show("Вы ввели неверный VIN");
            if (reg.Count() > 8 || reg.Count() < 8)
                MessageBox.Show("Вы ввели неверный регистрационный номер");
            if (familia == "" || imya == "" ||
                otchestvo == "" || vin == "" || reg == "")
                MessageBox.Show("Вы заполнили не все поля");
            if (TypeTC == "")
                MessageBox.Show("Вы не заполнили тип транспортного средства");
            if (photo == null)
                MessageBox.Show("Вы не загрузили изображение");


            XDocument xdoc = new XDocument();
            XElement person = new XElement("person");
            XAttribute familiaAttr = new XAttribute("name1", familia);
            XElement personimyaElem = new XElement("name2", imya);
            XElement personotchestvoElem = new XElement("name3", otchestvo);
            XElement persondataRElem = new XElement("data", dataR);
            XElement personvinElem = new XElement("vin", vin);
            XElement personregElem = new XElement("reg", reg);
            XElement persontypeTCElem = new XElement("typeTC", TypeTC);
            person.Add(familiaAttr);
            person.Add(personimyaElem);
            person.Add(personotchestvoElem);
            person.Add(persondataRElem);
            person.Add(personvinElem);
            person.Add(personregElem);
            person.Add(persontypeTCElem);
            XElement persons = new XElement("persons");
            persons.Add(person);
            xdoc.Add(persons);
            xdoc.Save("persons.xml");

            MessageBox.Show("Data saved");
        }

        private void buttonPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                pictureBoxPhoto.Image = System.Drawing.Image.FromFile(selectedImagePath);
                pictureBoxPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void Print()
        {
            string filePath = "PTC.pdf";
            Document document = new Document(PageSize.A4.Rotate());

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();
                PdfContentByte content = writer.DirectContent;
                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bf, 12);
                foreach (Control control in Controls)
                {
                    if(control is Panel panel)
                    {
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is DateTimePicker dateTimePicker)
                            {
                                string text = dateTimePicker.Text;
                                float x = dateTimePicker.Location.X + 3;
                                float y = dateTimePicker.Location.Y + 3;

                                content.BeginText();
                                content.SetFontAndSize(bf, 12);
                                content.SetTextMatrix(x, document.PageSize.Height - y);
                                content.ShowText(text);
                                content.EndText();
                            }
                            if (panelControl is TextBox textBox)
                            {
                                string text = textBox.Text;
                                float x = textBox.Location.X + 3;
                                float y = textBox.Location.Y + 3;

                                content.BeginText();
                                content.SetFontAndSize(bf, 12);
                                content.SetTextMatrix(x, document.PageSize.Height - y);
                                content.ShowText(text);
                                content.EndText();
                            }
                            else if (panelControl is Label label && label!=label5)
                            {
                                string text = label.Text;
                                float x = label.Location.X;
                                float y = label.Location.Y;

                                content.BeginText();
                                content.SetFontAndSize(bf, 12);
                                content.SetTextMatrix(x, document.PageSize.Height - y);
                                content.ShowText(text);
                                content.EndText();
                            }
                            else if (panelControl is GroupBox groupBox)
                            {
                                foreach (Control groupBoxControl in groupBox.Controls)
                                {
                                    if (groupBoxControl is RadioButton radioButton && radioButton.Checked)
                                    {
                                        string text = radioButton.Text;
                                        float x = groupBox.Location.X+3;
                                        float y = groupBox.Location.Y+3;

                                        content.BeginText();
                                        content.SetFontAndSize(bf, 12);
                                        content.SetTextMatrix(x, document.PageSize.Height - y);
                                        content.ShowText(text);
                                        content.EndText();
                                    }
                                }
                            }
                        }
                    }                   

                }
            }
            catch (DocumentException de)
            {
                MessageBox.Show(de.Message);                
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
            finally
            {
                document.Close();
            }

            MessageBox.Show("Data saved");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
