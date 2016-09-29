"use strict";
var pdfPageSetup = (function () {
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
    var pageGeometry = function (pageSize, landscape, headerHeight, footerHeight) {

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
            case 'A4': pageRect = A4pageRectangle;
                break;
            case 'A3': pageRect = A3pageRectangle;
                break;
            default:
                pageRect = A4pageRectangle;
        }
        var contentRect = new geom.Rect(
                     [margins.Left, margins.Top + headerHeight + LINE_SPACING],
                     [pageRect.getSize().getWidth() - margins.Left - margins.Right, pageRect.getSize().getHeight() - margins.Top - margins.Bottom - headerHeight - footerHeight - 2 * LINE_SPACING]);

        var printableRect = new geom.Rect(
                [margins.Left, margins.Top],
                [pageRect.getSize().getWidth() - margins.Left - margins.Right, pageRect.getSize().getHeight() - margins.Top - margins.Bottom]);


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
                [printableRect.width(), footerHeight]),

        }
    };

    var formatPage = function (content, pageNumber, totalPages, title) {
        // calculate page geometry
        if (content.options.landscape !== "false") {
            content.options.landscape = "true";
        }
        var pg = new pageGeometry("A4", content.options.landscape, mm(20), mm(10));

        var header = createHeader(pg, title, pageNumber, totalPages)
        var footer = createFooter(pageNumber, totalPages);
        var footerSeparator = createSeparator(pg.footerRect.getSize().getWidth());

        // Fit the content in the available space
        draw.fit(content, pg.contentRect);

        // Do a final layout with content
        var page = new draw.Layout(pg.pageRect, {
            // "Rows" go below each other
            orientation: "vertical",

            // Center rows relative to each other
            alignItems: "center",

            // Center the content block horizontally
            alignContent: "center",

            // Leave spacing between rows
            spacing: LINE_SPACING
        });

        //var clipPath = new draw.Path();
        //clipPath.moveTo(0, 30);

        //content.clip(clipPath);
        //draw.fit(header, pg.headerRect)
        page.append(header, content, footerSeparator, footer);

        //place elements of page to the corresponding rectangles
        draw.align([header], pg.headerRect, "start");
        draw.vAlign([header], pg.headerRect, "start");

        draw.align([content], pg.contentRect, "center");
        draw.vAlign([content], pg.contentRect, "start");

        // Move the footer to the bottom-right corner
        draw.align([footerSeparator], pg.footerRect, "start");
        draw.vAlign([footerSeparator], pg.footerRect, "start");

        draw.align([footer], pg.footerRect, "end");
        draw.vAlign([footer], pg.footerRect, "end");

        page.options.set("pdf.paperSize", "A4");
        if (content.options.landscape !== 'false') {
            page.options.set("pdf.landscape", "true");
        }


        return page;
    }

    function drawRectangle(p, originX, originY, height, width) {
        var p = new draw.Path({
            stroke: {
                color: "#999999",
                width: 0.5
            }
        });
        p.moveTo(originX, originY)
            .lineTo(originX + width, originY)
            .lineTo(originX + width, originY + height)
            .lineTo(originX, originY + height)
            .close();

        return p;
    }

    function createHeader(pageRect, title, pageNum, total) {
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
        var p4Width = heagerRect.size.width / 2;

        var p5OriginX = pOriginX + p4Width;
        var p5OriginY = p4OriginY;
        var p5Height = mm(7);
        var p5Width = heagerRect.size.width / 2;

        // The path is constructed using a chain of commands
        var path = drawRectangle(path, pOriginX, pOriginY, pHeight, pWidth);
        var path1 = drawRectangle(path1, p1OriginX, p1OriginY, p1Height, p1Width);
        var path2 = drawRectangle(path2, p2OriginX, p2OriginY, p2Height, p2Width);
        var path3 = drawRectangle(path3, p3OriginX, p3OriginY, p3Height, p3Width);
        var path4 = drawRectangle(path4, p4OriginX, p4OriginY, p4Height, p4Width);
        var path5 = drawRectangle(path5, p5OriginX, p5OriginY, p5Height, p5Width);


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
             '"ЛУКОЙЛ НЕФТОХИМ БУРГАС" АД',
            new geom.Point(mm(5), mm(11)),
            { font: mm(2.1) + "px 'DejaVu Sans'" }
        );

        var textTechnological = new draw.Text(
             'ТЕХНОЛОГИЧЕН ОТЧЕТ',
            new geom.Point(mm(5), mm(11)),
            { font: mm(4.5) + "px 'DejaVu Sans'" }
        );
        draw.align([textTechnological], path1.bbox(), "center");
        draw.vAlign([textTechnological], path1.bbox(), "center");

        var textDate = new draw.Text(
             'Дата: 01.06.2016 г.',
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


        var textFactory = new draw.Text(" Производство: Каталитична обработка на горивата",
            new geom.Point(mm(0), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textFactory], path4.bbox(), "start");
        draw.vAlign([textFactory], path4.bbox(), "center");

        var textPeriod = new draw.Text(" Период: Септември и от началото на годината",
            new geom.Point(mm(10), mm(0)),
            { font: mm(3) + "px 'DejaVu Sans'" }
        );
        draw.align([textPeriod], path5.bbox(), "start");
        draw.vAlign([textPeriod], path5.bbox(), "center");


        // Place all the shapes in a group
        var group = new draw.Group();
        group.append(path, image, text, path1, textTechnological, path2, textDate, path3, textPage, path4, textFactory, path5, textPeriod);

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
        var path = draw.Path.fromRect(borderRect, {
            stroke: {
                color: "#999999",
                width: 0.2
            }
        });
        return path;
    }

    function createFooter(page, total) {
        return new kendo.drawing.Text(
          kendo.format("{0} от {1}", page, total),
          new geom.Point(0, 0),
            {
                font: mm(3) + "px 'DejaVu Sans'"
            }
        );
    }

    return { FormatPage: formatPage }
}());