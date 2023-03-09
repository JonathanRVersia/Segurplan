$(".summernote").on("summernote.paste", function (e, ne) {
  let toReplace = $(
    (ne.originalEvent || ne).clipboardData.getData("text/html")
  );
  let table;
  toReplace.filter("table").each(function (i, val) {
    var tds = [];
    var tdHTML;
    var trs = [];
    var trHTML;

    for (var row of val.rows) {
      for (var cell of row.cells) {
        tds.push(
          '<td style="border: 1px solid black"><p>' +
            cell.innerText +
            "</p></td>"
        );
      }
      tdHTML = tds.join("");
      trs.push("<tr>" + tdHTML + "</tr>");
      tds = [];
    }
    trHTML = trs.join("");

    table =
      '<table class="table table-bordered" style="border: 1px solid black">' +
      trHTML +
      "</table>";
    val = $(table);
  });

  copyHtml(table != undefined ? table : toReplace);
});

function copyHtml(text) {
  const listener = function (ev) {
    ev.preventDefault();
    ev.clipboardData.setData("text/html", text);
    ev.clipboardData.setData("text/plain", text);
  };
  document.addEventListener("copy", listener);
  document.execCommand("copy");
  document.removeEventListener("copy", listener);
}
