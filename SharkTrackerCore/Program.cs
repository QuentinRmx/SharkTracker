using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SharkTrackerCore.LoRDeckCodes;

namespace SharkTrackerCore
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            // SharkTracker tracker = SharkTracker.New();

            // await tracker.UpdateFromRiot((sender, eventArgs) =>
            // {
            //     try
            //     {
            //         if (!(sender is RiotDownloader dl))
            //             return;
            //         Console.WriteLine(
            //             $"Current progress: {dl.ProgressPercent:F2}% ({dl.Progress}/{dl.TotalProgress} files)");
            //     }
            //     catch (Exception e)
            //     {
            //         Console.WriteLine(e);
            //     }
            // });
            await TryGetCollectionFromMobalytics();
        }

        private static void TryDecodeCollection()
        {
            List<CardCodeAndCount> cards = LoRDeckEncoder.GetDeckFromCode(
                "CEKQCAYDCIBAGBQGBYBAGBARCIBAGAAGBQEQGAQBAIBQQCIKBMIRECQCAIAQEAYEAUDAOCAJBIFAEBABAIBQIBIGA4EASCQKAIA" +
                "ACAQDAQCQMBYIBEFAUAQFAEBAGBAFAYDQQCIKBIBACAICAMCAKBQHBAEQUCQDAUBAGBAFAYDQUDANBYFAEAYBAIBQIBIGA4EASC" +
                "QKAMAQEBAFAYDQQCQLCEJDAAYJAMDAODAOB4IBCEYUCULRUGY4DUQSGJBGE4USYLZQGM3DQOR6H5DUQSKLJRIVEVCVKZLVQWK4L" +
                "ZQGENABAEAQGBAFAYDQQCIKBMGA2DQQCEJBGFAVCYLRQGI2DMOR4IBBEIRSIJJGE4UCSKRLFUXC6MBRGIZTINJWG44DSNIBAAAQ" +
                "EAYEAYDQSCQLBQGQ4DYRCIJRIFIWC4MBSGQ3DQOR4HZAEERCGJBFEYTSQKJKFMWC2LRPGAYTEMZUGU3DOOBVAECACAQDAQCQMBY" +
                "IBEFAYDIOCAIREEYUCULRQGI2DMOB2HQ7EERCGJBGE4UCUKZMFUXC6MBRGIZTINJWG44DSOR3GUAQEAICAMCAKBQHBAEQUCYMBU" +
                "HA6EARCIJRIFIWC4MBUGY4DUPB6IBBEQSSMJZIFEVCWLBNFYXTAMJSGQ2TMNZYHE2QCAYCAMCAKBQHBAEQUCYMBUHA6EARCMKBK" +
                "FQXDAMRUGY5DYPSAIJCEMSCKJRHFAUSUKZMFUXC6MBRGIZTINJWG44DKAIFAEBQIBIGA4EQUCYMBYHRAEQTCQKRMFYZDINRYHI6" +
                "D4QCCIRDEQSSMJZIFEVCWLBNFYXTAMJSGM2DKNRXHA4TUPQCAYAQEAYEAUDAOCAJBIFQYDIOB4IBCEQTCQKRMFYYDENBWHA5DYP" +
                "SAIJCEMSCKJRHFAUSUKZMFUXC6MBRGIZTKNRXHA4TUOZ4HU7D6AQBAMBBGAYDBEBBSPACAEBQGEIBAEAR6");
            cards.ForEach(c => Console.WriteLine($"{c.CardCode}: {c.Count}"));
        }

        static async Task TryGetCollectionFromMobalytics()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync("https://lor.mobalytics.gg/api/v2/riot/cards");
                Console.WriteLine(await resp.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}