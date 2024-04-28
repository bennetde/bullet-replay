extends Button

@export
var play_button: CompressedTexture2D

@export
var pause_button: CompressedTexture2D

func _on_toggled(toggled_on):
	icon = play_button if !toggled_on else pause_button
	pass # Replace with function body.
