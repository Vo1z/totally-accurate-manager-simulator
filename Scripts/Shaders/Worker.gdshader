shader_type canvas_item;
uniform vec4 workerColor;

void fragment()
{
	COLOR = texture(TEXTURE, UV);
	if(COLOR == vec4(1))
	{
		COLOR = workerColor;
	}
}
