#version 330

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;

// Input uniform values
uniform mat4 mvp;
uniform float time;

// Output vertex attributes (to fragment shader)
out vec2 fragTexCoord;
out vec4 fragColor;

float rand(vec2 co){
    return fract(sin(dot(co, vec2(12.9898, 78.233))) * 43758.5453);
}

void main()
{
    // Send vertex attributes to fragment shader
    fragTexCoord = vertexTexCoord;
    fragColor = vertexColor;

    float timeA = floor(time * 5.0) / 5.0;
    float timeB = ceil(time * 5.0) / 5.0;

    vec2 seedA = vec2(timeA) + gl_VertexID;
    vec2 seedB = vec2(timeB) + gl_VertexID;

    vec3 offsetA = vec3(vec2(rand(seedA), rand(seedA + 0.1)), 0.0) * 2.0;
    vec3 offsetB = vec3(vec2(rand(seedB), rand(seedB + 0.1)), 0.0) * 2.0;

    float t = (time - timeA) / (timeB - timeA);
    vec3 offset = mix(offsetA, offsetB, t);

    // Calculate final vertex position
    gl_Position = mvp*vec4(vertexPosition + offset, 1.0);
}