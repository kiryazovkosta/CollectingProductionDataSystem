"use strict";
var pdfPageSetup = (function() {
    // ----------------- pdf page setup Begin

    // Import Drawing API namespaces
    var draw = kendo.drawing;
    var geom = kendo.geometry;

    // See
    // http://docs.telerik.com/kendo-ui/framework/drawing/drawing-dom#dimensions-and-css-units-for-pdf-output
    function mm(val) {
        return val * 2.8347;
    }

    // Spacing between header, content and footer
    var LINE_SPACING = mm(5);

    // A4 Sheet with 1 cm borders
    var pageGeometry = function(pageSize, landscape, headerHeight, footerHeight) {

        var A4pageRectangle = new geom.Rect([mm(0), mm(0)], [mm(210), mm(297.5)]);
        var A3pageRectangle = new geom.Rect([mm(0), mm(0)], [mm(297.5), mm(420)]);
        var margins = { Left: mm(20), Top: mm(10), Bottom: mm(10), Right: mm(10) };


        if (landscape === "true") {
            A4pageRectangle = new geom.Rect([mm(0), mm(0)], [mm(297.5), mm(210)]);
            A3pageRectangle = new geom.Rect([mm(0), mm(0)], [mm(420), mm(297.5)]);
            margins = { Left: mm(10), Top: mm(20), Bottom: mm(10), Right: mm(10) };
        }

        var pageRect;
        switch (pageSize) {
        case 'A4':
            pageRect = A4pageRectangle;
            break;
        case 'A3':
            pageRect = A3pageRectangle;
            break;
        default:
            pageRect = A4pageRectangle;
        }
        var contentRect = new geom.Rect(
            [margins.Left, margins.Top + headerHeight + LINE_SPACING],
            [
                pageRect.getSize().getWidth() - margins.Left - margins.Right,
                pageRect.getSize().getHeight() -
                margins.Top -
                margins.Bottom -
                headerHeight -
                footerHeight -
                2 * LINE_SPACING
            ]);

        var printableRect = new geom.Rect(
            [margins.Left, margins.Top],
            [
                pageRect.getSize().getWidth() - margins.Left - margins.Right,
                pageRect.getSize().getHeight() - margins.Top - margins.Bottom
            ]);


        return {
            pageRect: pageRect.clone(),
            margins: margins,
            printableRect: printableRect,
            headerRect: new geom.Rect(
                [margins.Left, margins.Top],
                [printableRect.width(), headerHeight]),
            contentRect: contentRect.clone(),
            footerRect: new geom.Rect(
                [margins.Left, margins.Top + headerHeight + contentRect.getSize().getHeight() + 2 * LINE_SPACING],
                [printableRect.width(), footerHeight])
        }
    };

    var formatPage = function(content, pageNumber, totalPages, exportData) {
        // calculate page geometry

        if (content.options.landscape !== "false") {
            content.options.landscape = "true";
        }
        var pg = new pageGeometry("A4", content.options.landscape, mm(20), mm(10));

        var header = createHeader(pg, exportData, pageNumber, totalPages);
        var footer = createFooter(pg);

        // Do a final layout with content
        var page = new draw.Layout(pg.pageRect,
        {
            // "Rows" go below each other
            orientation: "vertical",

            // Center rows relative to each other
            alignItems: "center",

            // Center the content block horizontally
            alignContent: "center",

            // Leave spacing between rows
            spacing: LINE_SPACING
        });

        if (pageNumber === totalPages) {
            var spacing = mm(2);
            pg.contentRect = new geom.Rect([pg.contentRect.getOrigin().x, pg.contentRect.getOrigin().y],
                [pg.contentRect.getSize().width, pg.contentRect.getSize().height - mm(17) - spacing]);
            var signRect = new geom.Rect([pg.footerRect.getOrigin().x, pg.footerRect.getOrigin().y - mm(17) - spacing],
                [pg.printableRect.size.width, mm(17)]);
            var signContent = createSignSegment(pg, exportData);

            draw.align([signContent], signRect, "end");
            draw.vAlign([signContent], signRect, "start");
            page.append(signContent);

        }

        // Fit the content in the available space
        draw.fit(content, pg.contentRect);

        page.append(header, content, footer);

        //place elements of page to the corresponding rectangles
        draw.align([header], pg.headerRect, "start");
        draw.vAlign([header], pg.headerRect, "start");

        draw.align([content], pg.contentRect, "center");
        draw.vAlign([content], pg.contentRect, "start");

        // Move the footer to the bottom-right corner
        //draw.align([footerSeparator], pg.footerRect, "start");
        //draw.vAlign([footerSeparator], pg.footerRect, "start");

        draw.align([footer], pg.footerRect, "center");
        draw.vAlign([footer], pg.footerRect, "center");

        page.options.set("pdf.paperSize", "A4");
        if (content.options.landscape !== 'false') {
            page.options.set("pdf.landscape", "true");
        }

        return page;
    }

    function drawRectangle(originX, originY, height, width) {
        var p = new draw.Path({
            stroke: {
                color: "#999999",
                width: 0.75
            }
        });
        p.moveTo(originX, originY)
            .lineTo(originX + width, originY)
            .lineTo(originX + width, originY + height)
            .lineTo(originX, originY + height)
            .close();

        return p;
    }

    function createHeader(pageRect, exportData, pageNum, total) {
        //width = width * 1.1;

        var heagerRect = pageRect.headerRect;

        var pOriginX = heagerRect.origin.x - pageRect.margins.Left;
        var pOriginY = heagerRect.origin.y - pageRect.margins.Top;
        var pHeight = mm(14);
        var pWidth = mm(40);

        var p1OriginX = pOriginX + pWidth;
        var p1OriginY = pOriginY;
        var p1Height = mm(14);
        var p2Width = mm(40);
        var p1Width = heagerRect.size.width - pWidth - p2Width;

        var p2OriginX = p1OriginX + p1Width;
        var p2OriginY = p1OriginY;
        var p2Height = mm(7);

        var p3OriginX = p2OriginX;
        var p3OriginY = p1OriginY + p2Height;
        var p3Height = mm(7);
        var p3Width = mm(40);

        var p4OriginX = pOriginX;
        var p4OriginY = pOriginY + pHeight;
        var p4Height = mm(7);
        var p5Width = mm(40);
        var p4Width = heagerRect.size.width - p5Width;

        var p5OriginX = pOriginX + p4Width;
        var p5OriginY = p4OriginY;
        var p5Height = mm(7);


        // The path is constructed using a chain of commands
        var path = drawRectangle(pOriginX, pOriginY, pHeight, pWidth);
        var path1 = drawRectangle(p1OriginX, p1OriginY, p1Height, p1Width);
        var path2 = drawRectangle(p2OriginX, p2OriginY, p2Height, p2Width);
        var path3 = drawRectangle(p3OriginX, p3OriginY, p3Height, p3Width);
        var path4 = drawRectangle(p4OriginX, p4OriginY, p4Height, p4Width);
        var path5 = drawRectangle(p5OriginX, p5OriginY, p5Height, p5Width);


        // This rectangle defines the image position and size
        var imageRect = new geom.Rect(
            new geom.Point(mm(3), mm(1.5)),
            new geom.Size(mm(34), mm(8))
        );

        // Create the image
        var imageUrl = window.location.origin + '/Content/Images/LUK.png';
        var image = new draw.Image(imageUrl, imageRect);

        // Create the text
        var text = new draw.Text(
            '"ЛУКОЙЛ Нефтохим Бургас" АД',
            new geom.Point(mm(2), mm(11)),
            { font: mm(2.3) + "px 'DejaVu Sans'" }
        );

        var textTechnological = new draw.Text(
            'ТЕХНОЛОГИЧЕН ОТЧЕТ',
            new geom.Point(mm(5), mm(11)),
            { font: mm(4.5) + "px 'DejaVu Sans'" }
        );
        draw.align([textTechnological], path1.bbox(), "center");
        draw.vAlign([textTechnological], path1.bbox(), "center");

        var dateString = exportData.Month.toLocaleDateString('bg-BG');
        if (dateString.split('.')[0].length === 1) {
            dateString = '0' + dateString;
        }
        var textDate = new draw.Text(
            'Дата: ' + dateString,
            new geom.Point(mm(5), mm(11)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textDate], path2.bbox(), "center");
        draw.vAlign([textDate], path2.bbox(), "center");

        var textPage = new draw.Text(
            'стр. ' + pageNum + ' от ' + total,
            new geom.Point(mm(5), mm(11)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPage], path3.bbox(), "center");
        draw.vAlign([textPage], path3.bbox(), "center");


        var textFactory = new draw.Text(" Производство: " + exportData.FactoryName,
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textFactory], path4.bbox(), "start");
        draw.vAlign([textFactory], path4.bbox(), "center");

        var textPeriod = new draw.Text(" Период: " + exportData.MonthAsString,
            new geom.Point(mm(10), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPeriod], path5.bbox(), "start");
        draw.vAlign([textPeriod], path5.bbox(), "center");

        // Place all the shapes in a group
        var group = new draw.Group();
        group.append(path,
            image,
            text,
            path1,
            textTechnological,
            path2,
            textDate,
            path3,
            textPage,
            path4,
            textFactory,
            path5,
            textPeriod);

        // Translate the group
        //group.transform(
        //    geom.transform().translate(20, 10)
        //);

        return group;


        //return new kendo.drawing.Text(title, new geom.Point(0, 0), {
        //    font: mm(5) + "px 'DejaVu Sans'"
        //});
    }

    function createSeparator(length) {
        var borderRect = new geom.Rect(
            new geom.Point(0, 0),
            new geom.Size(length, 0)
        );
        var path = draw.Path.fromRect(borderRect,
        {
            stroke: {
                color: "#999999",
                width: 0.75
            }
        });
        return path;
    }

    function createFooter(pg) {

        var fr = pg.footerRect.clone();
        fr.setOrigin(0, 0);
        var path = draw.Path.fromRect(fr,
        {

        });

        var footerSeparator = createSeparator(pg.footerRect.getSize().getWidth());
        draw.align([footerSeparator], path.bbox(), "start");
        draw.vAlign([footerSeparator], path.bbox(), "start");

        var footerFirstLine = new draw
            .Text('Този документ е издаден от "Система за автоматизирана производствена отчетност". ',
                new geom.Point(mm(5), mm(1)),
                {
                    font: 'italic ' + mm(2.4) + "px 'DejaVu Sans'",
                    fill: { color: '#999999' }
                }
            );
        draw.align([footerFirstLine], footerSeparator.bbox(), "center");
        //draw.vAlign([footerFirstLine], path.bbox(), "start");

        var footerSecondLine = new draw
            .Text('Документът е предназначен за служебно ползване. Забранява се копиране или предоставяне на външни лица без разрешение.',
                new geom.Point(mm(5), mm(4)),
                {
                    font: 'italic ' + mm(2.4) + "px 'DejaVu Sans'",
                    fill: { color: '#999999' }
                }
            );
        draw.align([footerSecondLine], footerSeparator.bbox(), "center");
        //draw.vAlign([footerSecondLine], path.bbox(), "end");

        var group = new draw.Group();
        group.append(footerSeparator, $.extend({}, footerFirstLine), $.extend({}, footerSecondLine));

        return group;
    }

    function createSignSegment(pageRect, exportData) {
        //width = width * 1.1;

        var heagerRect = pageRect.headerRect;

        var p1OriginX = heagerRect.origin.x - pageRect.margins.Left;
        var p1OriginY = heagerRect.origin.y - pageRect.margins.Top;
        var p1Height = mm(5);
        var p1Width = mm(20);

        var p2OriginX = p1OriginX + p1Width;
        var p2OriginY = p1OriginY;
        var p2Height = p1Height;
        var p2Width = mm(65);

        var p3OriginX = p2OriginX + p2Width;
        var p3OriginY = p1OriginY;
        var p3Height = p1Height;
        var p3Width = mm(75);

        var p4OriginX = p3OriginX + p3Width;
        var p4OriginY = p1OriginY;
        var p4Height = p1Height;
        var p4Width = mm(30);

        //var p5OriginX = p4OriginX + p4Width;
        //var p5OriginY = p1OriginY;
        //var p5Height = p1Height;
        //var p5Width = mm(30);

        var p6OriginX = p1OriginX;
        var p6OriginY = p1OriginY + p1Height;
        var p6Height = mm(7);
        var p6Width = p1Width;

        var p7OriginX = p2OriginX;
        var p7OriginY = p6OriginY;
        var p7Height = p6Height;
        var p7Width = p2Width;

        var p8OriginX = p3OriginX;
        var p8OriginY = p6OriginY;
        var p8Height = p6Height;
        var p8Width = p3Width;

        var p9OriginX = p4OriginX;
        var p9OriginY = p6OriginY;
        var p9Height = p6Height;
        var p9Width = p4Width;

        var p10OriginX = p1OriginX;
        var p10OriginY = p6OriginY + p6Height;
        var p10Height = mm(7);
        var p10Width = p1Width;

        var p11OriginX = p2OriginX;
        var p11OriginY = p10OriginY;
        var p11Height = p10Height;
        var p11Width = p2Width;

        var p12OriginX = p3OriginX;
        var p12OriginY = p10OriginY;
        var p12Height = p10Height;
        var p12Width = p3Width;

        var p13OriginX = p4OriginX;
        var p13OriginY = p10OriginY;
        var p13Height = p10Height;
        var p13Width = p4Width;

        // The path is constructed using a chain of commands

        var path1 = drawRectangle(p1OriginX, p1OriginY, p1Height, p1Width);
        var path2 = drawRectangle(p2OriginX, p2OriginY, p2Height, p2Width);
        var path3 = drawRectangle(p3OriginX, p3OriginY, p3Height, p3Width);
        var path4 = drawRectangle(p4OriginX, p4OriginY, p4Height, p4Width);
        //var path5 = drawRectangle(p5OriginX, p5OriginY, p5Height, p5Width);
        var path6 = drawRectangle(p6OriginX, p6OriginY, p6Height, p6Width);
        var path7 = drawRectangle(p7OriginX, p7OriginY, p7Height, p7Width);
        var path8 = drawRectangle(p8OriginX, p8OriginY, p8Height, p8Width);
        var path9 = drawRectangle(p9OriginX, p9OriginY, p9Height, p9Width);
        var path10 = drawRectangle(p10OriginX, p10OriginY, p10Height, p10Width);
        var path11 = drawRectangle(p11OriginX, p11OriginY, p11Height, p11Width);
        var path12 = drawRectangle(p12OriginX, p12OriginY, p12Height, p12Width);
        var path13 = drawRectangle(p13OriginX, p13OriginY, p13Height, p13Width);

        var textPath2 = new draw.Text("Име, Фамилия",
            new geom.Point(mm(0), mm(0)),
            { font: 'bold ' + mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath2], path2.bbox(), "center");
        draw.vAlign([textPath2], path2.bbox(), "center");

        var textPath3 = new draw.Text("Длъжност",
            new geom.Point(mm(0), mm(0)),
            { font: 'bold ' + mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath3], path3.bbox(), "center");
        draw.vAlign([textPath3], path3.bbox(), "center");

        var textPath4 = new draw.Text("Дата",
            new geom.Point(mm(0), mm(0)),
            { font: 'bold ' + mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath4], path4.bbox(), "center");
        draw.vAlign([textPath4], path4.bbox(), "center");

        //var textPath5 = new draw.Text("Подпис",
        //    new geom.Point(mm(0), mm(0)),
        //    { font: mm(3) + "px 'DejaVu Sans'" }
        //);
        //draw.align([textPath5], path5.bbox(), "center");
        //draw.vAlign([textPath5], path5.bbox(), "center");

        var textPath6 = new draw.Text("Съставил",
            new geom.Point(mm(0), mm(0)),
            { font: 'bold ' + mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath6], path6.bbox(), "center");
        draw.vAlign([textPath6], path6.bbox(), "center");

        // Todo: add text to path from 7 to 9 from json data in form

        var textPath7 = new draw.Text(exportData.CreatorName || "",
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath7], path7.bbox(), "center");
        draw.vAlign([textPath7], path7.bbox(), "center");

        var textPath8 = new draw.Text(exportData.Occupation || "",
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath8], path8.bbox(), "center");
        draw.vAlign([textPath8], path8.bbox(), "center");

        var date = "";
        if (exportData.DateOfCreation !== undefined && exportData.DateOfCreation !== null) {
            date = exportData.DateOfCreation.toLocaleDateString('bg-BG');
        }

        var textPath9 = new draw.Text(date,
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath9], path9.bbox(), "center");
        draw.vAlign([textPath9], path9.bbox(), "center");


        var textPath10 = new draw.Text("Утвърдил",
            new geom.Point(mm(0), mm(0)),
            { font: 'bold ' + mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath10], path10.bbox(), "center");
        draw.vAlign([textPath10], path10.bbox(), "center");

        var textPath11 = new draw.Text(exportData.ApproverName || "",
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath11], path11.bbox(), "center");
        draw.vAlign([textPath11], path11.bbox(), "center");

        var textPath12 = new draw.Text(exportData.ApproverOccupation || "",
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath12], path12.bbox(), "center");
        draw.vAlign([textPath12], path12.bbox(), "center");

        var approvementDate = "";
        if (exportData.DateOfApprovement !== undefined && exportData.DateOfApprovement !== null) {
            approvementDate = exportData.DateOfApprovement.toLocaleDateString('bg-BG');
        }

        var textPath13 = new draw.Text(approvementDate,
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPath13], path13.bbox(), "center");
        draw.vAlign([textPath13], path13.bbox(), "center");

        // Place all the shapes in a group
        var group = new draw.Group();
        group.append(path1,
            path2,
            textPath2,
            path3,
            textPath3,
            path4,
            textPath4, /*path5, textPath5,*/
            path6,
            textPath6,
            path7,
            textPath7,
            path8,
            textPath8,
            path9,
            textPath9,
            path10,
            textPath10,
            path11,
            textPath11,
            path12,
            textPath12,
            path13,
            textPath13);

        // Translate the group
        //group.transform(
        //    geom.transform().translate(20, 10)
        //);

        return group;


        //return new kendo.drawing.Text(title, new geom.Point(0, 0), {
        //    font: mm(5) + "px 'DejaVu Sans'"
        //});
    }

    function getPdfExportData(selector) {
        var text = $(selector).val();
        var result = JSON.parse(text,
            function(key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
                        /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(+a[1],
                            +a[2] - 1,
                            +a[3],
                            +a[4],
                            +a[5],
                            +a[6]);
                    }
                }
                return value;
            });
        return result;
    }

    return {
        FormatPage: formatPage,
        GetPdfExportData: getPdfExportData
    }
}());