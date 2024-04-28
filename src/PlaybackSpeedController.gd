class_name PlaybackSpeedController
extends MenuButton

var _currently_selected = 1

signal on_playback_speed_changed(speed: float)

func _ready():
	get_popup().connect("index_pressed", on_index_pressed)
	
func on_index_pressed(index):
	var speed = 1;
	match index:
		0:
			speed = 0.5
		1:
			speed = 1
		2:
			speed = 2
		3:
			speed = 4
		4:
			speed = 8
		5:
			speed = 16
	
	get_popup().set_item_checked(_currently_selected, false)
	get_popup().set_item_checked(index, true)
	_currently_selected = index
	text = "%sx" % speed
	on_playback_speed_changed.emit(speed)
	pass
