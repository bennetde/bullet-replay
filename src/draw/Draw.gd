extends Node2D

@onready
var _lines := $Lines

@export
var camera: Camera2D

var _pressed := false
var _current_line: Line2D

func _process(_delta):
	if Input.is_action_just_pressed("draw_undo"):
		var children_count = _lines.get_child_count()
		if children_count == 0: return
		_lines.get_child(children_count - 1).queue_free()

func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		_pressed = event.pressed && event.button_index == MOUSE_BUTTON_LEFT
		
		if _pressed:
			_current_line = Line2D.new()
			_current_line.width = 5
			_lines.add_child(_current_line)
		pass
		
	if event is InputEventMouseMotion && _pressed:
		var pos = camera.get_global_mouse_position()
		_current_line.add_point(pos)
		pass
