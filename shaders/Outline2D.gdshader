shader_type canvas_item;

uniform vec3 outline_color: source_color;
uniform float outline_width;

void vertex() {
	// Called for every vertex the material is visible on.
}

vec2 uv_scale(vec2 in_uv, float scale) {
	vec2 out_uv = in_uv - vec2(.5);
	out_uv *= scale;
	out_uv += vec2(.5);
	return out_uv;
}

float step1(float x) {
	return smoothstep(0.0, 1.0, x);
}

void fragment() {
	float normal = texture(TEXTURE, UV).a;
	
	float outline = 0.0;
	outline += texture(TEXTURE, UV - vec2(-outline_width, 0.0)).a;
	outline += texture(TEXTURE, UV - vec2(outline_width, 0.0)).a;
	outline += texture(TEXTURE, UV - vec2(0.0, outline_width)).a;
	outline += texture(TEXTURE, UV - vec2(0.0, -outline_width)).a;
	outline += texture(TEXTURE, UV - vec2(-outline_width, outline_width)).a;
	outline += texture(TEXTURE, UV - vec2(outline_width, outline_width)).a;
	outline += texture(TEXTURE, UV - vec2(-outline_width, -outline_width)).a;
	outline += texture(TEXTURE, UV - vec2(outline_width, -outline_width)).a;
	outline = clamp(outline, 0.0, 1.0);
	outline -= normal;
	
	vec3 outline_col = outline_color * outline;
	vec3 normal_col = COLOR.rgb * normal;
	COLOR = vec4(outline_col + normal_col, 1.0);
}

void light() {
	// Called for every pixel for every light affecting the CanvasItem.
}
