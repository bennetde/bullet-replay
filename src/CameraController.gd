extends Camera2D

@export
var sensitivity: float = 1.0
@export
var zoom_strength: float = 1.0

var _mouse_input: Vector2 = Vector2.ZERO

func _process(_delta):
	
	if Input.is_action_pressed("zoom_in"):
		zoom += Vector2.ONE * zoom_strength
	#print(zoom_input)
	#zoom += Vector2.ONE * zoom_input * zoom_strength
	
	if Input.is_action_pressed("should_move"):
		global_position -= _mouse_input * sensitivity
	
	_mouse_input = Vector2.ZERO
	pass
	
func _input(event):
	var zoom_input := Input.get_axis("zoom_in", "zoom_out")
	if zoom_input < 0:
		zoom *= 1.1
	if zoom_input > 0:
		zoom /= 1.1
		
	if event is InputEventMouseMotion:
		_mouse_input = event.relative
