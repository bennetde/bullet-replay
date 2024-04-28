extends MenuButton

var file_dialog: FileDialog

@export
var open_shortcut: Shortcut

@export
var quit_shortcut: Shortcut


func _ready():
	file_dialog = $FileDialog
	var popup = get_popup()
	popup.id_pressed.connect(on_id_pressed)
	
	# Add Load Button
	popup.add_item("Load Demo...", 0, (KEY_MASK_CTRL | KEY_O) as Key)
	#popup.set_item_shortcut(popup.get_item_index(0), )
	
	#
	var menu = PopupMenu.new()
	#menu.set_name("Open Recent")
	menu.name = "OpenRecentMenu"
	menu.add_item("Placeholder", 2)
	popup.add_child(menu)
	popup.add_submenu_item("Open Recent", "OpenRecentMenu")
	
	# Add Quit Button
	popup.add_item("Quit", 1, (KEY_MASK_CTRL | KEY_Q) as Key)
	
func on_id_pressed(id: int):
	match id:
		0:
			file_dialog.show()
		1:
			get_tree().quit()
		_:
			push_warning("Unknown id pressed ", id)
	pass
