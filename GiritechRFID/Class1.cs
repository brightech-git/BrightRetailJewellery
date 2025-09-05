using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C002R;
using System.Timers;

namespace GiritechRFID
{
    public class Class1
    {

        public class GetData
        {
            private static System.Timers.Timer timer;
            C002 reader = new C002();
            private static bool Inventory = false;
            //To hold the previous ID and PreviousIDCount
            public static string[] prevID = new string[50];
            public static int prevIDCount = 0;
            int cnt = 1;
            public List<string> list;
            public void CatchData()
            {
                string returnValue = reader.initRfidReader("192.168.0.150", "9000");
                if (returnValue != "Success")
                {
                    MessageBox.Show("Init Failed" + returnValue);
                }

                reader.startTagDetection();
                SetTimer();
                Inventory = true;

            }

            public void SetTimer()
            {

                timer = new System.Timers.Timer(100);
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Enabled = true;
            }

            public void StopTimer()
            {
                try
                {
                    timer.Stop();
                    reader.closeRfidReader();
                }
                catch (Exception ex)
                {

                }
                //// Stop tag detection in reader
                //reader.stopTagDetection();
                //timer.Stop();
                //Inventory = false;
            }

            public void OnTimedEvent(object source, ElapsedEventArgs e)
            {
                if (Inventory == true)
                {
                    // Check if any tag is detected
                    if (reader.tagDetection())
                    {
                        // Read the Tag ID
                        string Item;
                        while ((Item = reader.tagRead()) != "")
                        {
                            bool present = false;
                            for (int i = 0; i < prevID.Length; i++)
                            {
                                if (Item == prevID[i])
                                {
                                    present = true;
                                    break;
                                }
                            }
                            // else add in the list view
                            if (present == false)
                            {
                                list = new List<string>();
                                ////////////list.Add(cnt.ToString());
                                list.Add(Item);
                                //this.dataGridView1.Rows.Add(list.ToArray());
                                prevID[prevIDCount++] = Item;
                                cnt++;
                            }
                        }
                    }
                }
            }

        }

    }
}
