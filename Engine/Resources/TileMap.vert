#version 400

layout(location = 0) in vec2 position;

uniform ivec2 topleft;

out vec2 fragTexcoord;

void main()
{
    gl_Position = vec4(position + topleft, 0, 1);
    fragTexcoord = position;
}
