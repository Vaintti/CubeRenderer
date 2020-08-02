#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 transform;
uniform mat4 timeRotation;

out vec3 Normal;

void main()
{
	gl_Position = transform * vec4(aPosition, 1.0);
	Normal = vec3(timeRotation * vec4(aNormal, 0.0));
}