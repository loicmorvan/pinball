#version 400

layout(location = 0) in vec2 position;

uniform vec2 topleft;
uniform vec4 camera;
uniform vec2 size;

out vec2 fragTexcoord;

void main()
{
    gl_Position = vec4(2 * (((size * position + topleft) - camera.xy) / camera.zw) - 1, 0, 1);
    fragTexcoord = position;
}
