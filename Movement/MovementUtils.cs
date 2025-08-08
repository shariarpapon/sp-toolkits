using UnityEngine;

namespace SPToolkits.Movement
{
    /// <summary>
    /// Contains utility functions for movement calculations.
    /// </summary>
    public static class MovementUtils
    {
        public const float CC_SKIN_CLEARANCE = 0.002f;
        public const float CC_ORIGIN_CLEARANCE = 0.25f;
        public const float GROUND_CHECK_DEPTH = 0.1f;

        /// <summary>
        /// Calculates the initial jump speed required to reach the peak at the height.
        /// </summary>
        /// <returns>Initial speed of the jump needed to reach the maximum height.</returns>
        public static float CalculateJumpSpeed(float gravity, float peakHeight)
        {
            return Mathf.Sqrt(2 * gravity * peakHeight);
        }

        /// <summary>
        /// Calculates the total time taken to travel some linear distance.
        /// </summary>
        /// <returns>The travel duration</returns>
        public static float CalculateLinearTravelDuration (float distanceTraveled, float travelSpeed)
        {
            return distanceTraveled / travelSpeed;
        }

        ///<summary>Shoots a ray in the local down direction from the colliders center in world space and checks if a gameobject is detected underneath.</summary>
        /// <param name="collider">This colliders bounds will be used to get the ray casting origin.</param>
        /// <returns>
        /// <br>The layer index if one is detected underneath.
        /// <br/>-1 if nothing is detected underneath.</br>
        /// </returns>
        public static int GetLayerUnderneath(Collider collider) 
        {
            Ray ray = new()
            {
                origin = collider.transform.TransformPoint(collider.bounds.center),
                direction = -collider.transform.up
            };
            if (Physics.Raycast(ray, out RaycastHit hit))
                return hit.transform.gameObject.layer;

            return -1;
        }

        /// <summary>
        /// Checks if the bottom-center of the controllers body is on the ground.
        /// </summary>
        /// <param name="controller">The character controller</param>
        /// <param name="whatIsGround">Any layer considered ground in this context.</param>
        /// <param name="maxStableSlopeAngle">Controls the maximum ground slope angle needed to be considered grounded.</param>
        /// <returns>True if grounded.</returns>
        public static bool IsCenterGrounded(CharacterController controller, LayerMask whatIsGround, float maxStableSlopeAngle) 
        {
            return IsCenterGrounded(controller, whatIsGround, maxStableSlopeAngle, GROUND_CHECK_DEPTH);
        }

        /// <summary>
        /// Checks if the bottom-center of the controllers body is on the ground.
        /// </summary>
        /// <param name="controller">The character controller</param>
        /// <param name="whatIsGround">Any layer considered ground in this context.</param>
        /// <param name="maxStableSlopeAngle">Controls the maximum ground slope that will be considered grounded.</param>
        /// <param name="checkDepth">Controls how far below the controllers bottom the ray checks.</param>
        /// <returns>True if grounded.</returns>
        public static bool IsCenterGrounded(CharacterController controller, LayerMask whatIsGround, float maxStableSlopeAngle, float checkDepth)
        {
            HitInfo info = RayDown(controller, whatIsGround, checkDepth);
            if (info == null || Vector3.Angle(controller.transform.up, info.normal) > maxStableSlopeAngle)
                return false;

            return true;
        }

        ///<summary>
        /// <br>Shoots a single ray downwards from the center of the controller's position to a specified depth and checks if ground is hit.
        /// <br/>The down direction is relative to the controller’s local space.</br>
        /// </summary>
        public static HitInfo RayDown(CharacterController controller, LayerMask layerMask, float extraDepth = 0)
        {
            Ray ray = new() 
            {
                origin = GetDownCastOrigin(controller, CC_ORIGIN_CLEARANCE - CC_SKIN_CLEARANCE),
                direction = -controller.transform.up
            };

            if (Physics.Raycast(ray, out RaycastHit hit, CC_ORIGIN_CLEARANCE + controller.skinWidth + extraDepth, layerMask))
                return new HitInfo(hit.point, ray.origin, ray.direction, hit.normal);
            return null;
        }

        ///<summary>
        /// <br>Casts a sphere downwards from the center of the controller and checks if ground is hit.
        /// <br/>The down direction is relative to the controller’s local space.</br>
        /// </summary>
        public static HitInfo SphereDown(CharacterController controller, float depth, LayerMask layerMask)
        {
            float detectionRadius = controller.radius + controller.skinWidth;
            Ray ray = new()
            {
                origin = GetDownCastOrigin(controller, depth),
                direction = -controller.transform.up
            };
            if (Physics.SphereCast(ray, detectionRadius, out RaycastHit hit, depth, layerMask))
                return new HitInfo(hit.point, ray.origin, ray.direction, hit.normal);

            return null;
        }

        /// <summary>
        /// Casts a specified number of rays radially from a specified origin.
        /// </summary>
        /// <param name="origin">Origin of all the rays.</param>
        /// <param name="axis">Axis around which the rays are shot.</param>
        /// <param name="radius">The maximum length of the rays.</param>
        /// <param name="rayCount">The number of rays (must be greater than 0).</param>
        public static HitInfo RadialRaycast(Vector3 origin, Vector3 axis, float radius, uint rayCount, LayerMask layerMask) 
        {
            Vector3 plane = Vector3.Angle(axis, Vector3.up) <= 0.002f ? Vector3.forward : Vector3.Cross(axis, Vector3.up);
            return RadialRaycast(origin, axis, plane, radius, rayCount, layerMask);
        }

        /// <summary>
        /// Casts a specified number of rays radially from a specified origin and around a specified axis.
        /// </summary>
        /// <param name="origin">Origin of all the rays.</param>
        /// <param name="axis">Axis around which the rays shoot.</param>
        /// <param name="plane">Any arbitrary vector perpendicular to the axis vector.</param>
        /// <param name="radius">The maximum length of the rays.</param>
        /// <param name="rayCount">The number of rays.</param>
        /// <returns>The HitInfo of the closest object hit.</returns>
        public static HitInfo RadialRaycast(Vector3 origin, Vector3 axis, Vector3 plane, float radius, uint rayCount, LayerMask layerMask)
        {
            plane.Normalize();

            Ray ray = new() { origin = origin };
            RaycastHit hit;

            float angleStep = 360.0f / rayCount;
            float minSqrDist = float.MaxValue;
            HitInfo hitInfo = null;

            for (uint i = 0; i < rayCount; i++)
            {
                float angle = i * angleStep;
                ray.direction = (Quaternion.AngleAxis(angle, axis) * plane).normalized;

                if (Physics.Raycast(ray, out hit, radius, layerMask))
                {
                    float sqrDist = (hit.point - origin).sqrMagnitude;
                    if (sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                        hitInfo = new HitInfo(hit.point, origin, ray.direction, hit.normal);
                    }
                }
            }
            return hitInfo;
        }

        /// <returns>Returns a safe raycasting origin based on the character controller and the spciefied offset to cast a ray downwards.</returns>
        private static Vector3 GetDownCastOrigin(CharacterController controller, float yOffset = 0) 
        {
            yOffset = (-controller.height * 0.5f) + yOffset;
            Vector3 localCotrollerBottom = controller.center + (controller.transform.up * yOffset);
            return controller.transform.TransformPoint(localCotrollerBottom);
        }

        /// <summary>
        /// Corrects the characters's motion when in proximity of collision to account for non-vertical and angled walls.
        /// </summary>
        /// <param name="controller">The controller to get the collision bounds from.</param>
        /// <param name="collisionLayer">The layers that will detect the collision.</param>
        /// <param name="currentVelocity">Current velocity before applying to the controller.</param>
        /// <param name="precision">This represents the number of rays that will be shot to detect collision. 
        ///                         <br>Higher number means more precision but less performance.</br></param>
        public static void CorrectCollisionProximityMotion(CharacterController controller, LayerMask collisionLayer, ref Vector3 currentVelocity, uint precision = 12)
        {
            float radius = controller.bounds.extents.x + controller.skinWidth + 0.01f;
            // Vector3 origin = GetDownCastOrigin(controller, -0.05f);
            Vector3 origin = GetDownCastOrigin(controller, 0.05f);
            HitInfo hit = RadialRaycast(origin, controller.transform.up, controller.transform.forward, radius, precision, collisionLayer);
            if (hit != null)
            {
                bool movingAwayFromClosestSurface = Vector3.Dot(hit.normal, currentVelocity.normalized) > 0;
                if (movingAwayFromClosestSurface)
                    return;
                Vector3 newMotion = Vector3.ProjectOnPlane(currentVelocity, hit.normal);
                currentVelocity = newMotion;
            }
        }

        /// <summary>
        /// Checks to see if the cahracter controller is too far off the edge and applies a slip motion if it is.
        /// </summary>
        /// <param name="controller">The character controller to which the motion will be applied to.</param>
        /// <param name="whatIsGround">Layers that are (or behave like) ground.</param>
        /// <param name="slipSpeed">The speed at which the character slips of the edge.</param>
        /// <param name="slipThreshold">The normalized distance of character's center from the edge of the ground before slipping.</param>
        /// <param name="grounded">Is the player grounded currently?</param>
        public static void AppplyEdgeProximitySlipToController(CharacterController controller, LayerMask whatIsGround, float slipSpeed, float slipThreshold, bool grounded)
        {
            if (grounded)
                return;

            float radialRayRadius = controller.bounds.extents.x + controller.skinWidth + 0.0001f;
            Vector3 radialRayOriign = GetDownCastOrigin(controller, -0.05f);
            HitInfo hitInfo = RadialRaycast(radialRayOriign, controller.transform.up, controller.transform.forward, radialRayRadius, 30, whatIsGround);
            if (hitInfo == null)
                return;

            float slipDistanceSqr = controller.radius * slipThreshold;
            slipDistanceSqr *= slipDistanceSqr;

            //TODO::
            //Consider checking this in local-space (convert contact to localspace, use transform.localPosition instead)
            Vector3 contact = hitInfo.impact;
            Vector3 center = controller.transform.position;
            Vector2 lateralDelta = new Vector2(contact.x, contact.z) - new Vector2(center.x, center.z);

            if (lateralDelta.sqrMagnitude > slipDistanceSqr)
            {
                Vector3 dir = (center - contact).normalized;
                controller.Move(dir * slipSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Spherically interpolate the transform's forward direction to the specified target forward.
        /// </summary>
        public static void SlerpRotateForward(Transform transform, Vector3 targetForward, float speed)
        {
            transform.forward = Vector3.Slerp(transform.forward, targetForward, speed * Time.deltaTime);
        }

        /// <param name="forwardSource">This vector will be projected on the lateral (XZ) plane of the transform's local-space to get a lateral forward direction.</param>
        /// <param name="transform">The transform of the local-space source.</param>
        /// <param name="direction">The direction in world space.</param>
        /// <returns>Normalized vector representing the direction orientated in local-space of the transform relative to the proejcted forward.</returns>
        public static Vector3 ToLocalDirectionWithProjectedForward(Transform transform, Vector3 forwardSource, Vector3 direction)
        {
            Vector3 projectedForward = Vector3.ProjectOnPlane(forwardSource, transform.up).normalized;
            return ToLocalDirectionWithForward(transform, projectedForward, direction);
        }

        /// <param name="transform">The transform of local-space source.</param>
        /// <param name="direction">The direction in world space.</param>
        /// <returns>Normalized vector representing the direction orientated in local-space relative to given forward direction.</returns>
        public static Vector3 ToLocalDirectionWithForward(Transform transform, Vector3 forward, Vector3 direction)
        {
            Vector3 localUnitVectorX = Vector3.Cross(transform.up, forward).normalized;
            return (localUnitVectorX * direction.x + transform.up * direction.y + forward * direction.z).normalized;
        }

        /// <param name="direction">The direction in global space.</param>
        /// <returns>Normalized vector representing the direction orientated in local space of the given transform.</returns>
        public static Vector3 ToLocalDirection(Transform transform, Vector3 direction)
        {
            return (transform.right * direction.x + transform.up * direction.y + transform.forward * direction.z).normalized;
        }
    }
}