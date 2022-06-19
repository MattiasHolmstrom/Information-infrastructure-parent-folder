using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LaborationCSN.Controllers
{
    public class CSNController : Controller
    {
        SQLiteConnection sqlite;

        public CSNController()
        {
            string path = HostingEnvironment.MapPath("/db/");
            sqlite = new SQLiteConnection($@"DataSource={path}\csn.sqlite");

        }
        XElement SQLResult(string query, string root, string nodeName)
        {
            sqlite.Open();

            var adapt = new SQLiteDataAdapter(query, sqlite);
            var ds = new DataSet(root);
            adapt.Fill(ds, nodeName);
            XElement xe = XElement.Parse(ds.GetXml());

            sqlite.Close();
            return xe;
        }


        //
        // GET: /Csn/Test
        // 
        // Testmetod som visar på hur ni kan arbeta från SQL till XML till
        // presentations-xml som sedan används i vyn.
        // Lite överkomplicerat för just detta enkla fall men visar på idén.
        public ActionResult Test()
        {
            string query = @"SELECT a.Arendenummer, s.Beskrivning, SUM(((Sluttid-starttid +1) * b.Belopp)) as Summa
                            FROM Arende a, Belopp b, BeviljadTid bt, BeviljadTid_Belopp btb, Stodform s, Beloppstyp blt
                            WHERE a.Arendenummer = bt.Arendenummer AND s.Stodformskod = a.Stodformskod
                            AND btb.BeloppID = b.BeloppID AND btb.BeviljadTidID = bt.BeviljadTidID AND b.Beloppstypkod = blt.Beloppstypkod AND b.BeloppID LIKE '%2009'
							Group by a.Arendenummer
							Order by a.Arendenummer ASC";
            XElement test = SQLResult(query, "BeviljadeTider2009", "BeviljadTid");
            XElement summa = new XElement("Total",
                (from b in test.Descendants("Summa")
                 select (int)b).Sum());
            test.Add(summa);

            // skicka presentations xml:n till vyn /Views/Csn/Test,
            // i vyn kommer vi åt den genom variabeln "Model"
            return View(test);
        }

        //
        // GET: /Csn/Index

        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Csn/Uppgift1

        public ActionResult Uppgift1()
        {
            string query = @"SELECT a.Arendenummer, u.UtbetDatum, u.UtbetStatus, SUM(((Sluttid-starttid +1) * b.Belopp)) as Summa, sf.Beskrivning
                             FROM UtbetaldTid_Belopp utb, Belopp b, UtbetaldTid ut, Utbetalning u, Arende a, Utbetalningsplan up, Stodform sf
                             WHERE utb.BeloppID = b.BeloppID AND utb.UtbetaldTidID = ut.UtbetTidID AND ut.UtbetID = u.UtbetID
                             AND a.Arendenummer = up.Arendenummer AND up.UtbetPlanID = u.UtbetPlanID AND sf.Stodformskod = a.Stodformskod
                             GROUP BY a.Arendenummer, u.UtbetDatum
                             ORDER BY u.UtbetDatum ASC";
            XElement u1 = SQLResult(query, "Utbetalningar", "UtbetalningSummering");

            // XElement total = new XElement("Total",
            //     (from b in test.Descendants("Summa")
            //      select (int)b).Sum());
            // test.Add(summa);
            //var u11 = from us in u1
            //          group us by us.

            var id = u1.Descendants("UtbetalningSummering").GroupBy(x => (string)x.Element("Arendenummer")).
                     Select(y => new XElement("Arende", y.OrderBy(z => z.Element("UtbetDatum").Value)));
            var result = new XElement(new XElement("Utbetalningar", id));

           foreach (XElement arende in result.Descendants("Arende"))
           {
                int utb = 0;
                int kvsum = 0;
                foreach(XElement utbsum in arende.Elements())
                {
                    if(utbsum.Element("UtbetStatus").Value == "Utbetald")
                    {
                        utb += (int)utbsum.Element("Summa");
                    }
                    else
                    {
                        kvsum += (int)utbsum.Element("Summa");
                    }
                }

                arende.Add(new XElement("Totsumma", utb + kvsum));
                arende.Add(new XElement("Utbetsumma", utb));
                arende.Add(new XElement("Kvarsumma", kvsum));
           }
    
            return View(result);
        }


        //
        // GET: /Csn/Uppgift2

        public ActionResult Uppgift2()
        {
            string query = @"SELECT u.UtbetDatum, bt.Beskrivning , SUM((Sluttid-starttid +1) * b.Belopp) as Summa, b.BeloppID
                            FROM Utbetalning u, UtbetaldTid ut, UtbetaldTid_Belopp utb, Belopp b, Beloppstyp bt
                            WHERE u.UtbetID = ut.UtbetID AND ut.UtbetTidID = utb.UtbetaldTidID AND utb.BeloppID = b.BeloppID
                            AND b.Beloppstypkod = bt.Beloppstypkod AND u.UtbetStatus = 'Utbetald'
                            GROUP BY u.UtbetDatum, bt.Beskrivning
                            ORDER BY u.UtbetDatum ASC";
            XElement u2 = SQLResult(query, "Utbetalningar", "UtbetalningSummering");

            var id = u2.Descendants("UtbetalningSummering").GroupBy(x => (string)x.Element("UtbetDatum")).
                     Select(y => new XElement("UtbetalningsDatum", y.OrderBy(z => z.Element("UtbetDatum").Value)));

            //var i2 = id.Descendants("Utbetalningsdatum").Elements("Utbetalningssummering").Sum(x => (int)x.Element("Summa"));
            
            //foreach (XElement utbd in id.Descendants("UtbetalningsDatum"))
            //{
            //    utbd.Add(new XElement("Totalsumma"),
            //        (from x in utbd.Elements("UtbetalningSummering")
            //         select (int)x.Element("Summa")).Sum());
            //}
            //
            var result = new XElement(new XElement("Utbetalningar", id));

            foreach (XElement utDatum in result.Descendants("UtbetalningsDatum"))
            {
                int tot = 0;
                foreach (XElement utbsum in utDatum.Elements())
                {
                    tot += (int)utbsum.Element("Summa");
                }

                utDatum.Add(new XElement("Totsumma", tot));
            }

            return View(result);
        }

        //
        // GET: /Csn/Uppgift3

        public ActionResult Uppgift3()
        {
            return View();
        }
    }
}