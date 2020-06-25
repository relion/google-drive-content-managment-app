/* Categories - Tree Menue */
/* by Ofek.M & Aryeh.T 8/2019-6/2020 */

var cat_json;
var reset = true;
var cat_json_cookie_name = "cat_json";
var cat_json_str = _getCookie(cat_json_cookie_name);
if (cat_json_str != "" && !reset) {
  cat_json = JSON.parse(cat_json_str);
} else {
  cat_json = [
    {
      "dname": "נושא",
      "content": [
        "משפחה 👩‍👩‍👦‍👦",
        {
          "dname": "מצחיק",
          "content": [
            "מצחיק 🙂",
            "פספוסים 😲",
            "סטנד-אפ 🤡"
          ]
        },
        {
          "dname": "ברכה 🙏",
          "content": [
            {
              "dname": "מהלך היום",
              "content": [
                "בוקר טוב",
                "יום טוב",
                "צהרים טובים",
                "ערב טוב",
                "לילה טוב 💤"
              ]
            },
            {
              "dname": "מהלך השבוע",
              "content": [
                "שבת שלום 🕯🕯",
                "שבוע טוב"
              ]
            },
            {
              "dname": "חגים",
              "content": [
                "חג שמח 🌙",
                "צום קל",
                "שנה טובה",
                "חודש טוב 🌚"
              ]
            },
            {
              "dname": "חיים",
              "content": [
                "יומולדת שמח 🎂",
                "בר מצוה",
                "נישואין "
              ]
            }
          ]
        },
        {
          "dname": "אוכל 🥪",
          "content": [
            "צמחוני\\טבעוני 🥑",
            "בשרי 🍖",
            "דגים ️🐟",
            "חלבי 🍕",
            "ביצים 🥚",
            "לחם ודגנים 🍞",
            "שתיה 🥛",
            "שתיה חריפה 🍾"
          ]
        },
        {
          "dname": "בריאות 👨‍⚕️",
          "content": [
            "רפואה קנוונציונלית",
            "תזונה נכונה 🥕",
            "פעילות ספורטיבית 🏃",
            "רפואה טבעית\\אלטרנטיבית",
            {
              "dname": "שלווה",
              "content": [
                "מדיטציה",
                "יוגה"
              ]
            }
          ]
        },
        {
          "dname": "רגש",
          "content": [
            "מרגש 🤧",
            "רומנטי\\זוגיות\\אהבה 💜",
            "הוריות\\ילדות 👩‍🍼👶",
            "מרגיע 😌",
            "נוסטלגיה",
            "חברות\\נאמנות 🤝",
            "סקסי 👄👙",
            "פרידה\\עצוב ☹️",
            "מדאיג 😟",
            "מפתיע 😯",
            "מעצבן 😡",
            "מפחיד 🎃"
          ]
        },
        {
          "dname": "רִשְעוּת",
          "content": [
            "מרושע 👿",
            "אלימות 💪🏽",
            "גניבה 👺"
          ]
        },
        "פוליטיקה 🦈",
        "צבאי 🎖️",
        "קורונה 😷",
        "פרסומות 💰",
        {
          "dname": "לימוד",
          "content": [
            "הוראה 👨🏿‍🏫",
            "אילוף",
            "מעורר מחשבה 🤔"
          ]
        },
        {
          "dname": "השקפת חיים",
          "content": [
            "פסיכולוגיה",
            "פילוסופיה 🎓",
            "אופטימית 👍",
            "פסימית 👎",
            "גלגול נשמות",
            {
              "dname": "בודהיזם 🕉️",
              "content": [
                "מהרישי",
                "אוֹשוֹ osho",
                "מוּג'י mooji"
              ]
            },
            {
              "dname": "דתי",
              "content": [
                "יהדות 🕎",
                "נצרות ✝️",
                "איסלם ☪️"
              ]
            }
          ]
        },
        {
          "dname": "מוזיקה ️🎼",
          "content": [
            {
              "dname": "שירה ️🎤",
              "content": [
                "זימרה",
                "אופרה",
                "חזנות",
                "שריקה\\קול"
              ]
            },
            {
              "dname": "מנגינה ️🎶",
              "content": [
                {
                  "dname": "קצב",
                  "content": [
                    "מהיר",
                    "בינוני",
                    "שקט"
                  ]
                },
                {
                  "dname": "סגנון",
                  "content": [
                    "מזרחי",
                    "פופ",
                    "גא'ז",
                    "בלוז",
                    "רוקנרול",
                    "תזמורת קלאסי",
                    "אלקטרוני",
                    "יהודי",
                    "דרום אמריקאי"
                  ]
                },
                {
                  "dname": "אוריינטלי",
                  "content": [
                    "ישראלי",
                    "רומני",
                    "אינדי",
                    "לטינו",
                    "מוזיקה קלאסית",
                    "מוזיקה צבאית"
                  ]
                }
              ]
            }
          ]
        },
        {
          "dname": "אומנות",
          "content": [
            "ציור 🎨",
            "פיסול",
            "קיפול נייר ✂",
            "אשליה אופטית",
            "פנטומימה",
            "אחר"
          ]
        },
        {
          "dname": "ריקודים 🕺",
          "content": [
            "סלוני",
            "בטן",
            "ריקודי עם",
            "דיסקו",
            "בלט 🩰",
            "על הקרח ️⛸️",
            "אקרובטיקה "
          ]
        },
        "קסמים 🎩",
        "סרט מצוייר\\אנימציה",
        {
          "dname": "מצגת שקופיות",
          "content": [
            "נוף",
            "עצמים",
            "טבע",
            "אנשים",
            "חיות",
            "ציורים"
          ]
        },
        {
          "dname": "טכנולוגיה ומדע",
          "content": [
            "אלקטרוניקה ומחשבים 🖥️",
            "כלים ממונעים 🛵🚘✈",
            "גננות\\חקלאות 🧑‍🌾",
            "בניה 👷🏗"
          ]
        }
      ]
    },
    {
      "dname": "משתתפים",
      "content": [
        {
          "dname": "בני אדם 👨‍👩‍👦‍👦",
          "content": [
            "תינוקות 👶",
            "ילדים 👦👧",
            "נערים\\בוגרים\\מבוגרים 👨👩",
            "זקנים 👴👵",
            "כללי"
          ]
        },
        {
          "dname": "חיות",
          "content": [
            "כלבים 🐶",
            "חתולים 🐈",
            "ציפורים 🐦",
            "דגים וחיות ים 🐠",
            "בהמות 🐮",
            "טורפים 🦁",
            "אחר"
          ]
        }
      ]
    },
    {
      "dname": "שפה",
      "content": [
        "עברית 🇮🇱",
        "אנגלית",
        "רומנית",
        "צרפתית",
        "יידיש",
        "קולות החיות",
        "אחר",
        {
          "dname": "כתוביות",
          "content": [
            "עברית",
            "אנגלית",
            "רומנית",
            {
              "dname": "איכות התרגום",
              "content": [
                "טובה",
                "בינונית",
                "גרועה"
              ]
            }
          ]
        }
      ]
    },
    {
      "dname": "דעתי",
      "content": [
        {
          "dname": "יחס",
          "content": [
            "אהבתי מאוד 👏",
            "אהבתי 👍",
            "ניטרלי 😕",
            "לא אהבתי 😳",
            "לא ראוי 🚫",
            "לא הבנתי 🙄"
          ]
        },
        {
          "dname": "האם ארצה לראות שוב",
          "content": [
            "כן 👍",
            "לא 👎",
            "אולי 😕"
          ]
        },
        "למחיקה 🗑️",
        "כפול"
      ]
    }
  ];
}

var cb_img_ar = [
  "TH_CB_empty.gif",
  "TH_CB_checked.gif",
  "TH_CB_checked_minus.gif",
  // "TH_CB_checked_or_indicates.gif",
];

//
var enable_edit = false;
var enable_files_checkbox = true;
var enable_folders_checkbox = true;
var use_trippleCheckbox = true;
var add_clear_buttton = true;

var checkboxes_paths_ht = {};

if (false)
  create_tree_menue(
    JSON.stringify(cat_json),
    enable_edit,
    enable_files_checkbox,
    enable_folders_checkbox,
    use_trippleCheckbox,
    add_clear_buttton
  );
// update_tree_menue_checkboxes({"f0": true, "f1": false});

var cat_root_ul = undefined;

function create_tree_menue(
  _json,
  _enable_edit,
  _enable_files_checkbox,
  _enable_folders_checkbox,
  _use_trippleCheckbox,
  _add_clear_buttton,
  background
) {
  enable_edit = _enable_edit;
  enable_files_checkbox = _enable_files_checkbox;
  enable_folders_checkbox = _enable_folders_checkbox;
  use_trippleCheckbox = _use_trippleCheckbox;
  add_clear_buttton = _add_clear_buttton;
  // alert(_json);
  json = JSON.parse(_json);
  // var json = JSON.parse(cat_json_str);
  if (cat_root_ul != undefined)
  {
    cat_root_ul.parentNode.removeChild(cat_root_ul);
  }
  var _cat_root_ul = document.createElement("ul");
  _cat_root_ul.style.display = "block"; // _cat_root_ul.setAttribute("style", "display: block");
  _cat_root_ul.setAttribute("class", "catRootUl");
  _cat_root_ul.style.backgroundColor = background; // #fff7b0;
  do_cat_helper(json, _cat_root_ul, "");
  var carets = _cat_root_ul.getElementsByClassName("caret");
  for (var i = 0; i < carets.length; i++) {
    var caret = carets[i];
    caret.onclick = function () {
      this.parentElement.querySelector(".nested").classList.toggle("active");
      this.classList.toggle("caret-down");
    };
  }
  if (add_clear_buttton) {
    var reset_button = document.createElement("button");
    reset_button.innerText = "reset";
    reset_button.onclick = function () {
      clear_all_tree_menue_triple_checkboxes();
      window.external.cat_tree_was_cleared();
    };
    var body_el = document.getElementsByTagName("BODY")[0];
    body_el.insertBefore(reset_button, body_el.firstChild);
  }
  document.getElementById("tree_menue_div").appendChild(_cat_root_ul);
  cat_root_ul = _cat_root_ul;
  return true;
}

function update_tree_menue_checkboxes(json_str) {
  // clear all checkboxes:
  for (var path in checkboxes_paths_ht) {
    checkboxes_paths_ht[path].checked = false;
  }
  //
  var json = JSON.parse(json_str);
  for (var path in json) {
    var cb = checkboxes_paths_ht["/" + path];
    // if (cb == null) return "checkbox with path: " + path + " not found.";
    cb.checked = json[path];
  }

  return true;
}

function clear_all_tree_menue_triple_checkboxes(json_str) {
  for (var path in checkboxes_paths_ht) {
    var cb = checkboxes_paths_ht[path];
    cb.i = 0;
    cb.src = "images/cb/" + cb_img_ar[0];
  }
}

function do_cat_helper(json, ul, path) {
  for (var i = 0; i < json.length; i++) {
    var jsonNode = json[i];
    var node = document.createElement("li");
    ul.appendChild(node);
    if (typeof jsonNode == "string") {
      // Leaf (File)
      if (enable_files_checkbox) {
        var _path = path + "/" + jsonNode;
        if (!use_trippleCheckbox) addCheckbox(node, _path);
        else addTrippleCheckbox(node, _path);
      }
      addFileNode(node, jsonNode);
      if (enable_edit) {
        addEditButtons(node);
      }
    } else {
      // Folder (Directory)
      node.setAttribute("class", "catFolder");
      var caret = document.createElement("span");
      caret.setAttribute("class", "caret");
      caret.appendChild(document.createTextNode(jsonNode.dname));
      node.appendChild(caret);
      if (enable_folders_checkbox) {
        var _path = path + "/" + jsonNode.dname;
        if (!use_trippleCheckbox) addCheckbox(node, _path);
        else addTrippleCheckbox(node, _path);
      }
      if (enable_edit) {
        addEditButtons(node);
      }
      var innerUl = document.createElement("ul");
      innerUl.setAttribute("class", "nested");
      node.appendChild(innerUl);
      do_cat_helper(jsonNode.content, innerUl, path + "/" + jsonNode.dname);
    }
  }

  function addTrippleCheckbox(node, path) {
    var checkbox = document.createElement("img");
    checkboxes_paths_ht[path] = checkbox;
    checkbox.style.verticalAlign = "middle";
    checkbox.style.cursor = "pointer";
    checkbox.i = 0;
    checkbox.src = "images/cb/" + cb_img_ar[0];
    checkbox.onclick = function () {
      this.i = (this.i + 1) % cb_img_ar.length;
      this.src = "images/cb/" + cb_img_ar[this.i];
      window.external.cat_has_checked(
        getFilePathString(this.parentElement),
        this.i
      );
    };
    node.appendChild(checkbox);
  }

  function addCheckbox(node, path) {
    var checkbox = document.createElement("input");
    checkboxes_paths_ht[path] = checkbox;
    checkbox.setAttribute("type", "checkbox");
    checkbox.onchange = function () {
      window.external.cat_has_checked(
        getFilePathString(this.parentElement),
        this.checked
      );
    };
    node.appendChild(checkbox);
  }

  function addEditButtons(node) {
    var editBtnClass = "editBtn editTreelBtn";
    var deleteBtnClass = "deleteBtn editTreelBtn";
    var deleteBtn = document.createElement("span");
    var editBtn = document.createElement("span");
    deleteBtn.setAttribute("class", deleteBtnClass);
    editBtn.setAttribute("class", editBtnClass);
    deleteBtn.node = node;
    deleteBtn.editBtn = editBtn;
    editBtn.node = node;
    node.appendChild(deleteBtn);
    node.appendChild(editBtn);
    deleteBtn.onclick = function () {
      node.parentNode.removeChild(node); // node.remove();
      this.editBtn.parentNode.removeChild(this.editBtn); // this.editBtn.remove();
      this.parentNode.removeChild(this); // this.remove();
      cat_to_json(cat_root_ul);
    };

    editBtn.onclick = function () {
      // alert("editBtn.onclick");
      var editInput = document.createElement("input");
      editInput.value = editBtn.node.getElementsByTagName("span")[0].innerText;
      editInput.editBtn = editBtn;
      editInput.setAttribute("type", "text");
      editBtn.setAttribute("class", "");
      editBtn.appendChild(editInput);
      editInput.focus();
      editInput.onkeydown = function (e) {
        // Enter is pressed
        if (e.keyCode == 13) {
          if (this.editBtn.node.classList.contains("catFile")) {
            this.editBtn.node.getElementsByClassName(
              "fileContentWrapper"
            )[0].innerHTML = editInput.value;
          } else {
            this.editBtn.node.firstChild.innerHTML = editInput.value;
          }
          cat_to_json(cat_root_ul);
          this.blur();
        }
        // Esc is pressed
        if (e.keyCode == 27) {
          this.blur();
        }
      };

      editInput.onclick = function () {};
      editInput.addEventListener("blur", function () {
        // alert("editInput.onblur");
        this.editBtn.setAttribute("class", editBtnClass);
        // cat_to_json(cat_root_ul); // lilo?
        this.parentNode.removeChild(this); // this.remove();
      });
    };
  }

  function addFileNode(node, jsonNode) {
    nodeWraperElement = document.createElement("span");
    nodeWraperElement.setAttribute("class", "fileContentWrapper");
    nodeWraperElement.appendChild(document.createTextNode(jsonNode));
    node.appendChild(nodeWraperElement);
    node.setAttribute("class", "catFile");
    nodeWraperElement.onclick = fileNode_onclick;
  }

  function fileNode_onclick() {
    var path_str = getFilePathString(this);
    cat_root_ul.pathContainer.innerHTML = path_str;
    cat_root_ul.pathContainer.v.json.cat_path_str = path_str;
    cat_root_ul.parentNode.removeChild(cat_root_ul); // cat_root_ul.remove();
  }

  var buttons = document.createElement("li");

  var addFile = document.createElement("span");
  addFile.setAttribute("class", "addFileBtn generalBtn");

  var addDir = document.createElement("span");
  addDir.setAttribute("class", "addDirBtn generalBtn");

  var input = document.createElement("input");
  input.setAttribute("type", "text");
  input.setAttribute("style", "display:none");
  if (enable_edit) {
    buttons.appendChild(addFile);
    buttons.appendChild(addDir);
  }

  buttons.appendChild(input);
  ul.appendChild(buttons);

  /* Add File/Dir Buttons EventListeners */

  addFile.onclick = function () {
    this.setAttribute("style", "display:none");
    addDir.setAttribute("style", "display:none");
    input.value = "";
    input.setAttribute("placeholder", "File Name, Enter to confirm");
    input.setAttribute("style", "display:inline-block");
    input.setAttribute("data-inputType", "file");
    input.focus();
  };
  //
  addDir.onclick = function () {
    this.setAttribute("style", "display:none");
    addFile.setAttribute("style", "display:none");
    input.value = "";
    input.setAttribute("placeholder", "Folder Name, Enter to confirm");
    input.setAttribute("style", "display:inline-block");
    input.setAttribute("data-inputType", "dir");
    input.focus();
  };
  //
  input.onkeydown = function (e) {
    if (e.key == "Enter") {
      type = this.getAttribute("data-inputType");
      var node = document.createElement("li");
      if (type == "file") {
        node = document.createElement("li");
        if (enable_files_checkbox) {
          addCheckbox(node);
        }
        addFileNode(node, this.value);
        if (enable_edit) {
          addEditButtons(node);
        }
        ul.insertBefore(node, buttons);
      } else if (type == "dir") {
        node.setAttribute("class", "catFolder");
        var caret = document.createElement("span");
        caret.setAttribute("class", "caret");
        caret.appendChild(document.createTextNode(this.value));
        node.appendChild(caret);
        ul.appendChild(node);
        if (enable_folders_checkbox) {
          addCheckbox(node);
        }
        if (enable_edit) {
          addEditButtons(node);
        }
        var innerUl = document.createElement("ul");
        innerUl.setAttribute("class", "nested");
        node.appendChild(innerUl);
        ul.insertBefore(node, buttons);
        do_cat_helper([], innerUl, path + "/" + this.value);
        toggleFolderView(caret);
      } else {
        throw "invalid type: " + type;
      }
      cat_to_json(cat_root_ul);
    } else if (e.key == "Escape") {
      this.setAttribute("style", "display:none");
    } else {
      return;
    }
    this.setAttribute("style", "display:none");
    addFile.setAttribute("style", "display:inline-block");
    addDir.setAttribute("style", "display:inline-block");
    this.value = "";
  };
  //
  input.addEventListener("focusout", function () {
    this.setAttribute("style", "display:none");
    addFile.setAttribute("style", "display:inline-block");
    addDir.setAttribute("style", "display:inline-block");
  });
}

function toggleFolderView(caret) {
  caret.addEventListener("click", function () {
    this.parentElement.querySelector(".nested").classList.toggle("active");
    this.classList.toggle("caret-down");
  });
}

function getFilePathString(node) {
  var catPath = [];
  var is_file = node.classList.contains("catFile");
  var x = node;
  if (is_file) {
    x = x.parentNode;
  }
  while (true) {
    if (
      x != undefined &&
      x.classList != undefined &&
      x.classList.contains("catFolder")
    ) {
      catPath.unshift(x.getElementsByClassName("caret")[0].innerText);
    } else if (x == cat_root_ul) {
      break;
    }
    x = x.parentNode;
  }
  if (is_file) {
    catPath.push(node.innerText);
  }
  return catPath.join("/");
}

/* Json utils */

function cat_to_json(ul) {
  var json = [];
  cat_to_json_helper(ul, json);
  var vson_str = JSON.stringify(json);
  _setCookie(cat_json_cookie_name, vson_str);
  window.external.cat_has_changed(JSON.stringify(json));
}

function cat_to_json_helper(ul, json) {
  for (var i = 0; i < ul.childNodes.length; i++) {
    var child = ul.childNodes[i];
    if (child.className == "catFile") {
      json.push(child.innerText);
    } else if (child.className == "catFolder") {
      var dir = {};
      dir.dname = child.getElementsByClassName("caret")[0].innerText;
      var dir_content = [];
      cat_to_json_helper(
        child.getElementsByClassName("nested")[0],
        dir_content
      );
      dir.content = dir_content;
      json.push(dir);
    }
  }
}

function _getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(";");
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == " ") {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

function _setCookie(cname, cvalue) {
  document.cookie = cname + "=" + cvalue;
  if (document.cookie.indexOf(cname + "=" + cvalue) == -1) {
    alert("cookie not set. but is: " + document.cookie);
  }
}
