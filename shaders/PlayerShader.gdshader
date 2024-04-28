shader_type canvas_item;

uniform float health_perc;
uniform vec3 rim_col: source_color;
uniform float rim_thickness;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 centerUV = UV - vec2(0.5f);
	float dist = length(centerUV);
	float angle = atan(-centerUV.x, centerUV.y) * 1.0 / (3.1416 * 2.0);
	angle += 0.5f;
	centerUV = (UV - vec2(0.5f));
	centerUV *= rim_thickness;
	centerUV += vec2(0.5f);
	
	float rim = texture(TEXTURE, centerUV).a;
	float center = texture(TEXTURE, UV).a;
	vec3 modulate = COLOR.rgb;
	rim -= center;
	rim *= step(angle, health_perc);
	
	vec4 rimCol = vec4(rim_col * rim, rim);
	vec4 centerCol = vec4(modulate * center, center);
	
	COLOR = rimCol + centerCol;
}

void light() {
	// Called for every pixel for every light affecting the CanvasItem.
}
