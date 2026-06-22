shader_type spatial;

uniform float amplitude = 0.2;
uniform float speed = 2.0;
uniform float phase = 0.0;

void vertex() {
	VERTEX.y += sin(TIME * speed + phase) * amplitude;
}
void fragment() {
	ALBEDO = vec3(0.0);
	METALLIC = 1.0;
	ROUGHNESS = 0.1;
}