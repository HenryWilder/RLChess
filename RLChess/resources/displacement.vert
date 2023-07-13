#version 330

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;

// Input uniform values
uniform mat4 mvp;
uniform int id;
uniform float time;
uniform float intensity;

const float freq = 5.0;
const float wavelen = 1.0f / freq;

// Output vertex attributes (to fragment shader)
out vec3 fragPosition;
out vec2 fragTexCoord;
out vec4 fragColor;
out vec3 fragNormal;

float rand(vec2 co){
    return fract(sin(dot(co, vec2(12.9898, 78.233))) * 43758.5453);
}

void main()
{
    // Send vertex attributes to fragment shader
    fragTexCoord = vertexTexCoord;
    fragColor = vertexColor;

    float timeA = floor(time * freq) / freq;
    float timeB =  ceil(time * freq) / freq;

    vec2 seedA = vec2(timeA + gl_VertexID + id);
    vec2 seedB = vec2(timeB + gl_VertexID + id);

    vec3 offsetA = vec3(rand(seedA) - 0.5, rand(seedA + 0.1) - 0.5, 0.0) * intensity * 2;
    vec3 offsetB = vec3(rand(seedB) - 0.5, rand(seedB + 0.1) - 0.5, 0.0) * intensity * 2;

    float t = (time - timeA) / wavelen;
    vec3 offset = mix(offsetA, offsetB, t);

    // Calculate final vertex position
    fragPosition = vec3(mvp * vec4(vertexPosition + offset, 1.0));
    fragTexCoord = vertexTexCoord;
    fragColor = vertexColor;
    
    mat3 normalMatrix = transpose(inverse(mat3(mvp)));
    fragNormal = normalize(normalMatrix * vertexNormal);

    // Calculate final vertex position
    gl_Position = mvp * vec4(vertexPosition + offset, 1.0);
}