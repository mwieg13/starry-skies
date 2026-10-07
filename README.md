# starry-skies
A network of docker containers acting as individual satellites, with a focus on avoiding collisions

# Key Technologies

- Docker: This project will utilize dozens to hundreds of docker containers
- Protobuf: This project will utilize protobuf to establish a common messaging format.
- Kafka: This project will use Kafka as a central messaging broker.

# Objectives

- Implement a system where many docker containers, representing satellites, are sharing comms through a message broker
- Be able to display all of their positions inside of a simple HTML webspace
- Implement basic collision-detection and collision-avoidance behavior
- Implement "explosions" to occur when two satellites collide

# Milestones for Development

1. Basic Comms.
    - Be able to have a docker container representing a satellite.
		- Be able to have it sending TLM data into a Kafka container.
		- Be able to subscribe to the TLM topic, and output the data (in a terminal is fine, initially).
		- Be able to show the satellite visually in a webpage.

2. Collision Detection.
    - Be able to detect if a collision has occurred.
		- If a collision occurs, publish on the collision topic and kill all of the involved containers.
		- Be able to visually show the death of a satellite.
		- Be able to spin up multiple satellites (n > 5)

3. Collision Avoidance
    - TODO

# Kafka Topics

1. TLM Downlink (TLM_BROADCAST)
    - publish svID, xPos, yPos, xVel, yVel

2. Emergency Broadcast (EMERGENCY_BROADCAST) (For collision detection)
		- publish svID, other_svID, requested_xVel, requested_yVel