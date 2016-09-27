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

    // A4 Sheet with 1 cm borders
    var PAGE_RECT = new geom.Rect(
      [mm(10), 0], [mm(210 - 20), mm(297 - 20)]
    );

    var PAGE_RECT_L = new geom.Rect(
      [mm(10), 0], [mm(297 - 20), mm(210 - 20)]
    );

    // Spacing between header, content and footer
    var LINE_SPACING = mm(5);

    var formatPage = function (content, pageNumber, totalPages, title) {
        var header = createHeader(title);
        var footer = createFooter(pageNumber, totalPages);

        var pageRect;
        if (content.options.landscape === 'false') {
            pageRect = PAGE_RECT;
        } else {
            pageRect = PAGE_RECT_L;
        }

        // Remove header, footer and spacers from the page size
        var contentRect = pageRect.clone();

        contentRect.size.height -= header.bbox().height() + footer.bbox().height() + 2 * LINE_SPACING;

        // Fit the content in the available space
        draw.fit(content, contentRect)

        // Do a final layout with content
        var page = new draw.Layout(pageRect, {
            // "Rows" go below each other
            orientation: "vertical",

            // Center rows relative to each other
            alignItems: "center",

            // Center the content block horizontally
            alignContent: "center",

            // Leave spacing between rows
            spacing: LINE_SPACING
        });
        page.append(header, content);
        //page.options.set("pdf.paperSize", "A4");
        page.reflow();
        draw.align([header], pageRect, "start");

        // Move the footer to the bottom-right corner
        page.append(footer);
        draw.vAlign([footer], pageRect, "end");
        draw.align([footer], pageRect, "end");

        return page;
    }

    function createHeader(title) {
        return new kendo.drawing.Text(title, [0, 0], {
            font: mm(5) + "px 'DejaVu Sans'"
        });
    }

    function createFooter(page, total) {
        return new kendo.drawing.Text(
          kendo.format("{0} от {1}", page, total),
          [0, 0], {
              font: mm(3) + "px 'DejaVu Sans'"
          }
        );
    }

    return { FormatPage: formatPage }
}());