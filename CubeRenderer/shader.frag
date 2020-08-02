#version 330 core
out vec4 FragColor;

in vec3 Normal;

void main()
{
	vec3 norm = normalize(Normal);	
	float diff = max(dot(norm, vec3(0.3, 1, 1)), 0.0);
	FragColor = vec4(0.4f, 0.5f, 0.6f, 1.0f) * diff;
}