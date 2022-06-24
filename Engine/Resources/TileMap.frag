#version 400

in vec2 fragTexcoord;

uniform sampler2D tex;

out vec4 FragColor;

void main()
{
    FragColor = texture(tex, fragTexcoord);
}
