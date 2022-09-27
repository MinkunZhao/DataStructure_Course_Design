namespace FSharpExt
open XPlot.Plotly
open System
open System.IO
open System.Diagnostics
module public Ext =
    let mutable count = 0;
    let ChartShow (s: string) (filename: string) =
        let path (subpath:string) = Environment.CurrentDirectory + "\\" + subpath
        let truepath = (path filename) + ".html"
        count <- count + 1
        let htmlhead = """
        <!DOCTYPE html>
        <html>
            <head>
                <meta charset="UTF-8" />
                <script src="./plotly-latest.min.js"></script>
            </head>
            <body>
        """
        let htmlbody = s;
        (*
        let htmlbody = """
                <div id="8962c9e4-69ae-4d50-8235-835474de6ca2" style="width: 700px; height: 500px;"></div>
                <script>
                    var data = [{"type":"scatter","x":[1,2,3,4],"y":[10,15,13,17]},{"type":"scatter","x":[2,3,4,5],"y":[16,5,11,9]}];
                   var layout = "";
                   Plotly.newPlot('8962c9e4-69ae-4d50-8235-835474de6ca2', data, layout);
                </script>
        """
        *)

        let htmltail = """
            </body>
        </html>
        """
        let html = htmlhead + htmlbody + htmltail
        File.WriteAllText(truepath, html)
        Process.Start(ProcessStartInfo(FileName = truepath, UseShellExecute = true)) |> ignore
    let CreateChart ticks fnlStatistics stlStatistics ttlStatistics filename =
        let trace1 =
            Scatter(
                x = ticks,
                y = fnlStatistics,
                marker = Marker(color = "rgb(0, 128, 0)"),
                name = "First nutritional level"
            )
        let trace2 =
            Scatter(
                x = ticks,
                y = stlStatistics,
                marker = Marker(color = "rgb(0, 0, 255)"),
                name = "Second trophic level"
            )
        let trace3 =
            Scatter(
                x = ticks,
                y = ttlStatistics,
                marker = Marker(color = "rgb(255, 0, 0)"),
                name = "Third trophic level"
            )
        let styledLayout =
            Layout(
                title = "Statistics Report",
                xaxis = Xaxis(title = "Tick"),
                yaxis = Yaxis(title = "Count")
            )
        let chart = [trace1; trace2; trace3]
                    |> Chart.Plot
                    |> Chart.WithLayout styledLayout
                    |> Chart.WithWidth 700
                    |> Chart.WithHeight 500
        ChartShow (chart.GetInlineHtml()) filename;